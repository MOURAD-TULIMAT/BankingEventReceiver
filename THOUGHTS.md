What things did you considered of during the implementation?

First thing I thought of was using polly package to insure resilience, and not reinvent the weel.
I also found it important to add a transactions table to keep track of them

Then I thought it would be best to use the Strategy Pattern that helps selecting the appropriate handler based on the MessageType enum for maintainability

In the DebitEventHandlerTests I checked for the error message so that I give an example of using the message handler directly in the unit tests

---

Anything was unclear?

Surely we still need to add the proper dependency injections to the program.cs file

also we could add more test cases, and more checks for the data according to your bussiness needs
Aor example your business might not allow a bank account balance to go bellow zero

and use the transactunal exeption in the proper places

but I don't think that that will be needed for this test
