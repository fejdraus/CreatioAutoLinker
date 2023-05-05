using System.Net;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public interface ICreatioService
{
    Task<bool> DoesPackageHaveParentById(string url, string body, string userName, string userPassword);
    Task<PackageProperties?> GetPackageProperties(string url, string body, string userName, string userPassword);
    Task<ResponseSavePackageProperties?> SavePackageProperties(string url, string body, string userName, string userPassword);
    Task<ListInPackages?> GetPackages(string url, string userName, string userPassword, string body = "");
    Task CreatioLogin(string url, string userName, string userPassword);
    Task<ValidateWorkspaceRoot?> ValidateWorkspace(string url, string userName, string userPassword, string body = "");
}