using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using IO = System.IO;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Analysis;
using Lucene.Net.Index.Memory;
using Version = Lucene.Net.Util.Version;
using System.IO;
using System.Web.Http;
using System.Web;

namespace Service.Pattern
{
    public class SelectedList
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
    public class SearchService<TEntity> : ISearchService<TEntity> where TEntity : IObjectState
    {
        public const int HitsLimit = 1000;
        public const int MaxSuggestWord = 3;
        public const int MaxSummaryLength = 200;

        //private static string _luceneDir = null;

        //private static FSDirectory _directoryTemp;

        //private static FSDirectory _directory
        //{
        //    get
        //    {
        //        if (!IO.Directory.Exists(_luceneDir))
        //            IO.Directory.CreateDirectory(_luceneDir);

        //        if (_directoryTemp == null)
        //            _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));

        //        if (IndexWriter.IsLocked(_directoryTemp))
        //            IndexWriter.Unlock(_directoryTemp);

        //        var lockFilePath = Path.Combine(_luceneDir, "write.lock");

        //        if (File.Exists(lockFilePath))
        //            File.Delete(lockFilePath);

        //        return _directoryTemp;
        //    }
        //}        

        public string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index");

        public FSDirectory _directoryTemp;

        public FSDirectory _directory
        {
            get
            {
                if (!IO.Directory.Exists(_luceneDir))
                    IO.Directory.CreateDirectory(_luceneDir);

                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(_luceneDir, "write.lock");

                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);

                return _directoryTemp;
            }
        }

        #region Private Fields
        
        private readonly IRepositoryAsync<TEntity> _repository;
        
        #endregion Private Fields

        #region Constructor
        
        protected SearchService(IRepositoryAsync<TEntity> repository) { _repository = repository; }
        public SearchService() { }
        
        #endregion Constructor

        public void _makeIndexFolder()
        {
            if (!IO.Directory.Exists(_luceneDir))
                IO.Directory.CreateDirectory(_luceneDir);
        }

        public bool _isLuceneIndexing() 
        {
            var lockFilePath = Path.Combine(_luceneDir, "write.lock");
            return File.Exists(lockFilePath); 
        }

        public Analyzer _getAnalyzer() 
        {
            return new Lucene.Net.Analysis.Shingle.ShingleAnalyzerWrapper(new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), MaxSuggestWord);
        }

        public IEnumerable<TEntity> GetAllIndexRecords() 
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<TEntity>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();

            // v 3.0.3: use 'hit.Doc'
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }

        public IEnumerable<TEntity> Search(string input, string fieldName = "") 
        {
            if (string.IsNullOrEmpty(input)) return new List<TEntity>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");

            input = string.Join(" ", terms);

            return _search(input, fieldName);
        }

        public IEnumerable<TEntity> SearchDefault(string input, string fieldName = "") 
        {
            return string.IsNullOrEmpty(input) ? new List<TEntity>() : _search(input, fieldName);
        }

        public IEnumerable<TEntity> _search(string searchQuery, string searchField = "") 
        {
            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = _getAnalyzer();

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    parser.AllowLeadingWildcard = true;
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = _mapLuceneToDataList(query, hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else  
                {
                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                                                                    new[] { IndexKey.Id, IndexKey.TextValue, IndexKey.TextContentId },
                                                                analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;
                    var results = _mapLuceneToDataList(query, hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        public Query parseQuery(string searchQuery, QueryParser parser) 
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        public IEnumerable<TEntity> _mapLuceneToDataList(IEnumerable<Document> hits) 
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();  
        }

        public IEnumerable<TEntity> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher) 
        {
            // v 3.0.3: use 'hit.Doc'
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        public IEnumerable<TEntity> _mapLuceneToDataList(Query query, IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(query, searcher.Doc(hit.Doc))).ToList();
        }

        public TEntity _mapLuceneDocumentToData(Query query, Document doc) 
        {
            TEntity obj = Activator.CreateInstance<TEntity>();
            return obj;
        }

        public TEntity _mapLuceneDocumentToData(Document doc) 
        {
            TEntity obj = Activator.CreateInstance<TEntity>();
            return obj;
        }

        public string GetSummaryWithHighlight(Query query, string text, string fileName) 
        {
            // create highlighter
            var analyzer = _getAnalyzer();
            var formatter = new SimpleHTMLFormatter("<span class=\"search-highlight\">", "</span>");
            var fragmenter = new SimpleFragmenter(250);
            var scorer = new QueryScorer(query);
            var highlighter = new Highlighter(formatter, scorer);
            highlighter.TextFragmenter = fragmenter;

            var stream = analyzer.TokenStream(fileName, new StringReader(text));
            var summary = highlighter.GetBestFragments(stream, text, 2, "...");

            if (string.IsNullOrEmpty(summary))
                summary = text.ToString();

            return summary;
        }

        public void AddUpdateLuceneIndex(TEntity data) 
        {
            AddUpdateLuceneIndex(new List<TEntity> { data });
        }

        public void AddUpdateLuceneIndex(IEnumerable<TEntity> sampleDatas) 
        {
            // init lucene
            var analyzer = _getAnalyzer();
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entries if any)
                foreach (var sampleData in sampleDatas) _addToLuceneIndex(sampleData, writer);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public void ClearLuceneIndexRecord(int record_id) 
        {
            // init lucene
            var analyzer = _getAnalyzer();
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term(record_id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public bool ClearLuceneIndex() 
        {
            try
            {
                var analyzer = _getAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Optimize() 
        {
            var analyzer = _getAnalyzer();
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        public void _addToLuceneIndex(TEntity data, IndexWriter writer) 
        {
            //var searchQuery = new TermQuery(new Term(data.Title));
            //writer.DeleteDocuments(searchQuery);

            //// add new index entry
            //var doc = new Document();

            //// add lucene fields mapped to db fields
            //doc.Add(new Field("Title", data.Title.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //doc.Add(new Field("Description", data.Description.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            //doc.Add(new Field("Url", data.Url.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            //// add entry to index
            //writer.AddDocument(doc);
        }

        public IEnumerable<string> _autoSuggest(string keyword, int maxSuggests) 
        {
            var result = new List<string>();

            keyword = keyword.Replace("\"", "").ToLower();

            if (string.IsNullOrEmpty(keyword))
                return result;

            using (var analyzer = _getAnalyzer())
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var reader = writer.GetReader();
                    var terms = reader.Terms(new Term(IndexKey.TextValue, keyword));
                    var count = 0;

                    while (terms.Term != null && terms.Term.Text.StartsWith(keyword) && count < maxSuggests)
                    {
                        result.Add(terms.Term.Text);
                        count++;

                        if (!terms.Next())
                            break;
                    }

                    terms.Dispose();
                    reader.Dispose();
                    analyzer.Close();
                }
            }

            return result;
        }

    }
}
