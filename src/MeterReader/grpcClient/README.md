## Generate code from protobuf
To run the powershell script for generating code from protobuf file, the script needs access to a couple dev dependencies in Node modules. 
Run this command in your terminal to get access, with this project as root

Powershell
```powershell
$env:Path = $env:Path + "./node_modules/.bin;./node_modules/grpc-tools/bin/;./node_modules/protoc-gen-grpc-web/bin/;"
```
**OR**

zsh/bash
```bash
export PATH=$PATH:./node_modules/.bin:./node_modules/grpc-tools/bin/:./node_modules/protoc-gen-grpc-web/bin/
```

## Bundle
To bundle javascript code with your dependencies and host it in our web app, use the following command using webpack, and outputting it to wwwroot/js/send as a file called send.js, built in a debuggable development mode.

```bash
npx webpack -o ../wwwroot/js/send/ ./send.js --mode development
```

