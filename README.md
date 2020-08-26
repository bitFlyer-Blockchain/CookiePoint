## CookiePoint

Sample demo project showing how to create a cryptocurrency wallet using miyabi Asset table.

The tutorial for this project can be found there :
https://medium.com/bitflyer-blockchain/how-to-build-simple-net-apps-using-miyabi-blockchain-part-1-bc686e01ab76

Please visit the [website](https://blockchain.bitflyer.com/miyabi/) for more information about miyabi or to request a demo.

Playground environment to try miyabi [there] (https://blockchain.bitflyer.com/miyabi/playground_agreement.html)

Documentation for miyabi [there] (https://blockchain.bitflyer.com/miyabi/manual/en/)

### Requirements
- miyabi v2.1.0 (SDK)
- netcoreapp3.0


### Get started

- update miyabisdkconfig.json with the correct miyabi host url (make sure this file can be found in path)
- update Program.cs to use a proper table admin private key.
```
        TABLE_ADMIN_PRIVATE_KEY = "input_your_key_here";
```            
- run main in Program.cs
