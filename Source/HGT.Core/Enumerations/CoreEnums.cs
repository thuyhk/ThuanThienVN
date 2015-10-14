using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core.Enums
{
    public enum RoleType
    {
        NormalUser = 16,
        Manager = 32,
        Administrator = 64,
        SA = 128
    }

    public enum EmailTemplateType
    {
        ForgetPass = 1,
        ConfirmOrder = 2,
        ContactForm = 3
    }

    public enum OrderStatusType
    {
        New = 1,
        Process = 2,
        Completed = 3,
        Cancel = 4
    }

    public enum ContentType
    {
        Introduction = 1,
        Contact = 2,
        Footer = 3,
        CreateOrderSuccess = 4,
        CreateOrderUnSuccess = 5
    }

    public enum SettingType
    {
        SiteSetting = 1,
        CategorySetting = 2,
        ProductSetting = 3
    }

    public enum ResultStatus
    {
        Fail = 0,
        Success = 1,
        CaptchaInCorrect = 2
    }
}
