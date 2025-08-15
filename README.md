# TextProcessing

This is the backend part of this excercise, it is build in:
- .NET 8
- nUnit
- Docker

# Steps to run locally

- clone this repo
- Build the project (vscode, visual studio, dotnet cli)
- open a terminal and go to the path where is the file `TextProcessing.sln`
- run below commands:

  ```
  docker build -f TextProcessing.Api/Dockerfile -t processingimag1 .
  
  docker run -d -p 8080:8080 --name processing_container processingimag1
  ```

- Test the API is responding with this request:
  
  ```
    http://localhost:8080/api/Processing/health
  ```
- Expected response
  ```
  {
    "status": "ok"
  }
  ```
- To test the Api for processing strings
  ```
     http://localhost:8080/api/Processing
    Body:
    {
      "input": "aabbc"
    }
  ```
