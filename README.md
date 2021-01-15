# Justification
1. Design patterns
The two design pattern used for creating transaction objects are builder and director pattern.
Since transaction has 4 different types and each type requires different components, it is a good practice to use builder to construct a set of parts and use director to assemble the parts into a complete transaction.
It reduces the code complexity in the client class and avoid exposing the actual implementation of creating transaction to client.

2. Class library
The functions handled by class library are validation for menu option and positive value input, SQL connection, data table generation, pagination, and customised exceptions.
This features are required in many places during coding. Therefore, it is ideal to use these methods in the class library to reduce code complexity and increase code reuse. These methods are very generalised and can be used in any program.

3. Async
The async and await are used when there are needs to read data from or write data into database where some delay may happen. When CustomerWebService read customer data from json and then write into database, LoginWebService can also read login data from json and then write into database concurrently. By using async methods, the total time needed to complete all read and write tasks is reduced.


Reference:
1. The REST API call references week 3 material "WebServiceAndDatabaseExample".
2. The builder and director design pattern references week 3 material "BuilderPatternDemo"
