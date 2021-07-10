cd /d %~dp0
protoc.exe -I=.--csharp_out=. --grpc_out=. --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe AccountService .proto
