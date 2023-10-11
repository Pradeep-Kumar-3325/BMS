## Building a Project

Build any .NET Core using the .NET Core CLI, which is installed with [the .NET Core SDK](https://www.microsoft.com/net/download). Then run
these commands from the CLI in the directory of the project(BMS.API):

```console
dotnet build
dotnet run
```
Then open browser and type 
`http://localhost:5165/swagger/index.html`

You can directly execute `BMS.API` Project fron Visual Studio 2022

To build and run:

1. Go to the project folder and build to check for errors:

    ```console
    dotnet build
    ```

2. Run your sample:

    ```console
    dotnet run
    ```
    # REST API

The REST API to the example app is described below.

```
    Account Type :- 0- Saving, 1- Current and 2 - Salary in Request Pass in String like "saving"
    Branch Name :- CRPF Camp
    Bank Name :- SBI
    Transaction Type :- 0- Withdraw, 1- Deposit 
```

## Create Account :- Create New Account and Customer can have multiple accounts just call this api with multiple times 

### Request

```
    Only Saving,Current and Salary Account Supported
    Bank and Branch Name should be sbi and crpf camp
```

`POST /api/Create`

    curl -i -H 'Accept: application/json' http://localhost:5165/api/create

Sample
    
   ```
{
  "accountType": "Saving",
  "balance": 100,
  "customer": {
    "userName": "pradeep",
    "email": "user@example.com",
    "phone": "+919953943788",
    "address": {
      "address1": "RA 72",
      "address2": "Delhi",
      "state": "Delhi",
      "pincode": 11002,
      "country": "India"
    }
  },
  "branchDetail": {
    "branchName": "crpf camp",
    "bankName": "sbi"
  }
}
```

### Response

    HTTP/1.1 200 OK
    Date: Thu, 24 Feb 2011 12:36:30 GMT
    Status: 200 OK
    Connection: close
    Content-Type: application/json
    Content-Length: 2
  ```
{
       "account": {
       "accountNumber": 1484542707,
       "customerId": 648701235,
       "branchId": 989792748,
       "balance": 100,
       "type": 0,
       "openingDate": "2023-10-11T17:52:03.4086043+05:30"
     },
     "validationMessage": null
   }
```

   ### Business Rule Vilation :- Response
   
     
      {
       "account": null,
       "validationMessage": "An account cannot have less than $100 at any time in an account!"
      }
    

## Transaction Withdraw
`
Please provide Welcome@123 as password and username of customer
`

### Request

`POST /api/withdraw`

    curl -i -H 'Accept: application/json' -d 'name=Foo&status=new' http://localhost:5165/api/withdraw
    
    
    {
      "amount": 10,
       "accountNumber": 1956076790,
       "userName": "pradeep",
       "password": "Welcome@123"
    }
    

### Response

    HTTP/1.1 201 Created
    Date: Thu, 24 Feb 2011 12:36:30 GMT
    Status: 201 Created
    Connection: close
    Content-Type: application/json
    Location: /thing/1
    Content-Length: 36

   ```
   {
    "transaction": {
    "transactionId": 1546995717,
    "type": 0,
    "amount": 10,
    "datetime": "2023-10-11T18:00:48.6625748+05:30",
    "accountNumber": 1956076790
    },
    "validationMessage": null
    }
  ```

### Transaction Deposit
`
Cannot deposit more than 10000 amount in single transaction
`

### Request

`POST /api/deposit`

    curl -i -H 'Accept: application/json' -d 'name=Foo&status=new' http://localhost:5165/api/deposit
    
    
    {
      "amount": 1000,
      "accountNumber": 1956076790
    }
    

### Response

    HTTP/1.1 201 Created
    Date: Thu, 24 Feb 2011 12:36:30 GMT
    Status: 201 Created
    Connection: close
    Content-Type: application/json
    Location: /thing/1
    Content-Length: 36

    
    {
     "transaction":
     {
      "transactionId": 277167246,
      "type": 1,
      "amount": 1000,
      "datetime": "2023-10-11T18:07:30.8490864+05:30",
      "accountNumber": 1956076790
     },
     "validationMessage": null
    }
    
   ```
