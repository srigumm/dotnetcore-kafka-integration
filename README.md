# Building realtime streaming applications using .NET Core and KAFKA


### User Scenario:


Let's take a simple use case of e-commerce company. Assume we are building a simple "Order management" APIs to sell products like "Unicorn Whistles".

 Our objective here is to build a fast & scalable backend APIs to take more order requests and quickly process or trigger other workflows to speedup the delivery process.

 To address scaling individual apps and other performance related key metrics, lets assume that we have decided to build the below two critical components
  - An Order API(RESTful) that takes users orders and responds back immediately with some acknowledgement info.
  - A background service that actually process these order requests.

### Archgitecutre diagram:
![architecture diagram](https://raw.githubusercontent.com/srigumm/dotnetcore-kafka-integration/master/Api/Images/architecture.png)

### Prerequisites:

 - VSCODE or some .net code editor
 - .NET Core 2.1
 - Docker
 - KAFKA Installation and Topics setup
 - Kafkacat command line tool
 - If you are connecting to Kerberos-aware KAFKA Enterprise Instance, ensure the below things:
    - Setup krb5.Conf file  with your organization's KDC details.
    - Keep krb5.conf file in default path i.e /etc/ or specify the path with KRB5_CONFIG environment variable.
    - Create Keytab file with your principal
    - Make sure your/service account has atleast read access to krb5.conf file.

### How to install KAFKA in local??
- It's easy to setup KAFKA in local using docker containers.
  Clone the below below repository and run "docker-compose up" command.
        commands:

             git clone https://github.com/TribalScale/kafka-waffle-stack.git
             cd kafaka-waffle-stack
             docker-compose up
   Above instructions should start a KAFKA server, and you can use the broker localhost:9092 to produce/consumer messages.
- Creating a new topic in local KAFKA:
    producing a sample message to a topic using kafkacat utility would create topic if it doesn't exist.
    so, run the below command and give some sample message like {"id":1234,"productname":"Unicorn Whistles","quantity":3}

       kafkacat -b localhost:9092 -t new_topic -P

### Implementation:

- Implemented a dotnetcore-WebApi post handler to capture user's order requests.( into "orderrequests" kafka topic)
- Implemented a background service(HostedService in .NET core) that process the user's order requests in the "orderrequests" kafka topic and writes to "readytoship" kafka topic.

### Run & Test:
1. Clone this repository:

       git clone https://github.com/srigumm/dotnetcore-kafka-integration.git
       cd dotnetcore-kafka-integration
2. Run the below commands at the root of your project folder.

       dotnet restore
       dotnet build
       dotnet run"

      This should start both webserver for our webapi rest service and  our background service(hosted inside the same webhost).

        WebApi URL: http://localhost:5000/api/order

3. Use postman to trigger "POST" calls to issue new order requests:
         http://localhost:5000/api/order

4. Verify if new messages were written to readytoship topic using kafkacat utility:

       kafkacat -b localhost:9092 -t readytoship -C

### Troubleshooting tips:
- If your producer/consumer is not responding at all, then verify your keytab file with below steps

      kinit username@MYDOMAIN.COM -k -t username.keytab
    you should get authenticated successfully (without being prompted for a password).
