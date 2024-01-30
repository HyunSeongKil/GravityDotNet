## 솔루션 생성

```
> dotnet new sln -o GravityDotNet
> cd GravityDotNet
```

## GravityCmmn

```
// 프로젝트 생성
GravityDotNet> dotnet new classlib -o GravityCmmn

// 솔루션에 추가
GravityDotNet> dotnet sln add .\GravityCmmn\GravityCmmn.csproj
```

## GravityCmmn Test

```
// 프로젝트 생성
GravityDotNet> dotnet new xunit -o GravityCmmn.Tests

// 솔루션에 추가
GravityDotNet> dotnet sln add .\GravityCmmn.Tests\GravityCmmn.Tests.csproj

// 참고 추가
GravityDotNet> cd GravityCmmn.Tests
GravityDotNet\GravityCmmn.Tests> dotnet add reference ..\GravityCmmn\GravityCmmn.csproj
```

## GravityFs

```
// 프로젝트 생성
GravityDotNet> dotnet new webapi --use-controllers -o GravityFs

// 솔루션에 추가
GravityDotNet> dotnet sln add .\GravityFs\GravityFs.csproj

// 참고 추가
GravityDotNet> cd GravityFs
GravityDotNet\GravityFs> dotnet add reference ..\GravityCmmn\GravityCmmn.csproj
```

## unit test

```
GravityDotNet> cd GravityCmmn.Tests
GravityDotNet\GravityCmmn.Tests> dotnet test
```
