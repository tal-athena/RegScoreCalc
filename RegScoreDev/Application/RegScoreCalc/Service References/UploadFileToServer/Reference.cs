﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegScoreCalc.UploadFileToServer {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UploadFileToServer.IFileTransferService")]
    public interface IFileTransferService {
        
        // CODEGEN: Generating message contract since the operation UploadFile is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileTransferService/UploadFile", ReplyAction="http://tempuri.org/IFileTransferService/UploadFileResponse")]
        RegScoreCalc.UploadFileToServer.UploadFileResponse UploadFile(RegScoreCalc.UploadFileToServer.RemoteFileInfo request);
        
        // CODEGEN: Generating message contract since the wrapper name (DownloadRequest) of message DownloadRequest does not match the default value (DownloadFile)
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileTransferService/DownloadFile", ReplyAction="http://tempuri.org/IFileTransferService/DownloadFileResponse")]
        RegScoreCalc.UploadFileToServer.RemoteFileInfo DownloadFile(RegScoreCalc.UploadFileToServer.DownloadRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RemoteFileInfo", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RemoteFileInfo {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string FileName;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string FolderName;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public long Length;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.IO.Stream FileByteStream;
        
        public RemoteFileInfo() {
        }
        
        public RemoteFileInfo(string FileName, string FolderName, long Length, System.IO.Stream FileByteStream) {
            this.FileName = FileName;
            this.FolderName = FolderName;
            this.Length = Length;
            this.FileByteStream = FileByteStream;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UploadFileResponse {
        
        public UploadFileResponse() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DownloadRequest", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class DownloadRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string FileName;
        
        public DownloadRequest() {
        }
        
        public DownloadRequest(string FileName) {
            this.FileName = FileName;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileTransferServiceChannel : RegScoreCalc.UploadFileToServer.IFileTransferService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileTransferServiceClient : System.ServiceModel.ClientBase<RegScoreCalc.UploadFileToServer.IFileTransferService>, RegScoreCalc.UploadFileToServer.IFileTransferService {
        
        public FileTransferServiceClient() {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RegScoreCalc.UploadFileToServer.UploadFileResponse RegScoreCalc.UploadFileToServer.IFileTransferService.UploadFile(RegScoreCalc.UploadFileToServer.RemoteFileInfo request) {
            return base.Channel.UploadFile(request);
        }
        
        public void UploadFile(string FileName, string FolderName, long Length, System.IO.Stream FileByteStream) {
            RegScoreCalc.UploadFileToServer.RemoteFileInfo inValue = new RegScoreCalc.UploadFileToServer.RemoteFileInfo();
            inValue.FileName = FileName;
            inValue.FolderName = FolderName;
            inValue.Length = Length;
            inValue.FileByteStream = FileByteStream;
            RegScoreCalc.UploadFileToServer.UploadFileResponse retVal = ((RegScoreCalc.UploadFileToServer.IFileTransferService)(this)).UploadFile(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RegScoreCalc.UploadFileToServer.RemoteFileInfo RegScoreCalc.UploadFileToServer.IFileTransferService.DownloadFile(RegScoreCalc.UploadFileToServer.DownloadRequest request) {
            return base.Channel.DownloadFile(request);
        }
        
        public string DownloadFile(ref string FileName, out long Length, out System.IO.Stream FileByteStream) {
            RegScoreCalc.UploadFileToServer.DownloadRequest inValue = new RegScoreCalc.UploadFileToServer.DownloadRequest();
            inValue.FileName = FileName;
            RegScoreCalc.UploadFileToServer.RemoteFileInfo retVal = ((RegScoreCalc.UploadFileToServer.IFileTransferService)(this)).DownloadFile(inValue);
            FileName = retVal.FileName;
            Length = retVal.Length;
            FileByteStream = retVal.FileByteStream;
            return retVal.FolderName;
        }
    }
}
