# .NET Core with Protocol Buffers
![](https://www.codespot.org/assets/cover/protocol-buffers.png)

### Step by step how to install protoc 
- download Protocol Buffers source from https://github.com/protocolbuffers/protobuf/releases
- run this commands to install protoc:
    - `./autogen.sh`
    - `./configure` 
    - `make` 
    - `make check`
    - `sudo make install`
    
- check version with command
    - `protoc --version`

### Generate proto class
- run the command to generate the proto class
    - `protoc --proto_path=Proto --csharp_out=Proto Proto/listing.proto`
