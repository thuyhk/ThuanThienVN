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

namespace Service.Pattern
{
    public interface ISearchService<TEntity> where TEntity : IObjectState
    {
        //FSDirectory _directoryTemp;
        //FSDirectory _directory;
        void _makeIndexFolder();
        bool _isLuceneIndexing();
        Analyzer _getAnalyzer();
        IEnumerable<TEntity> GetAllIndexRecords();
        IEnumerable<TEntity> Search(string input, string fieldName = "");
        IEnumerable<TEntity> SearchDefault(string input, string fieldName = "");
        IEnumerable<TEntity> _search(string searchQuery, string searchField = "");
        Query parseQuery(string searchQuery, QueryParser parser);
        IEnumerable<TEntity> _mapLuceneToDataList(IEnumerable<Document> hits);
        IEnumerable<TEntity> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher);
        IEnumerable<TEntity> _mapLuceneToDataList(Query query, IEnumerable<ScoreDoc> hits, IndexSearcher searcher);
        TEntity _mapLuceneDocumentToData(Query query, Document doc);
        TEntity _mapLuceneDocumentToData(Document doc);
        string GetSummaryWithHighlight(Query query, string text, string fileName);
        void AddUpdateLuceneIndex(TEntity data);
        void AddUpdateLuceneIndex(IEnumerable<TEntity> sampleDatas);
        void ClearLuceneIndexRecord(int record_id);
        bool ClearLuceneIndex();
        void Optimize();
        void _addToLuceneIndex(TEntity data, IndexWriter writer);
        IEnumerable<string> _autoSuggest(string keyword, int maxSuggests);

    }
}
