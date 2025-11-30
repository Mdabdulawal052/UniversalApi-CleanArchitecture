for Command 
dotnet new ca-usecase --name CreateCustomer --feature-name Customers --usecase-type command --return-type bool

for Query
dotnet new ca-usecase -n GetCustomers -fn Customers -ut query -rt CustomerDto
