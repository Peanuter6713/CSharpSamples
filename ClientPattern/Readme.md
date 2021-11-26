# 客户端模式  
***  
##1. 安装模板
	dotnet  new -i IdentityServer4.Templates

##2. 添加模板项目
	dotnet new is4empty -n Authorization.IdentityServer

##3. 添加解决方案
	dotnet  new sln -n Authorization.IdentityServer.Project  

	dotnet sln add Authorization.IdentityServer\Authorization.IdentityServer.csproj

	发现文档 https://localhost:5001/.well-known/openid-configuration  


# 密码模式    
	授权服务器 https://localhosot:5001
	Api资源 https://localhost:6001
***
# 密码模式客户端获取Token

# 基于内存的授权码实现
端口配置：MVC应用程序  https://localhost:7001/

授权服务器：https://localhost:5001/

Api资源： https://localhost:6001/

## 第一步：copy密码模式的代码到新的文件夹
## 第二步：打开新文件夹中的授权服务器项目
命令添加界面：  

	dotnet new is4ui

# 配置 (MvcClient)