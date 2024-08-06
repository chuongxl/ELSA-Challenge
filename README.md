# Real-Time Vocabulary Quiz Coding Challenge
## Overview

Welcome to the Real-Time Quiz coding challenge! Your task is to create a technical solution for a real-time quiz feature for an English learning application. This feature will allow users to answer questions in real-time, compete with others, and see their scores updated live on a leaderboard.

## Acceptance Criteria

1. **User Participation**:
   - Users should be able to join a quiz session using a unique quiz ID.
   - The system should support multiple users joining the same quiz session simultaneously.

2. **Real-Time Score Updates**:
   - As users submit answers, their scores should be updated in real-time.
   - The scoring system must be accurate and consistent.

3. **Real-Time Leaderboard**:
   - A leaderboard should display the current standings of all participants.
   - The leaderboard should update promptly as scores change.

### Part 1: System Design

1. **System Design Document**:
   - **Architecture Diagram**: 
   ![High level design](/Architecture/ELSA-HLD.drawio-5.svg)
   - **Component Description**: 
        
        1. **Client** : 
            Web App : Reactjs
            Mobile App : Native android and native Ios App

        2. **AUTH0**: https://auth0.com. Authentication and auhotization provider. This is third party servie to handle the registering, login and forgot password. This service will integrate with user management service (UMS) to sync the user information and user status.   

        3. **Google firebase** : For real-time message and push notification. 

        4. **Datadog** : Monitoring the whole application including infrastructure, application log, error tracking...etc

        5. **EKS Cluster**: The cluster for our microservice.

            5.1  **UMS** : User management system is to manage the user infromation. This service needs to integrate with the autho.com to handle the login, registering and forgot password feature. UMS also provider API for getting user's profile includinig score and their rank in the leaderboard.

            5.2 **Score service** : Provider api for the user submit their test result. To handle a large of number of quiz sessions, this service will forward the request to the 7.Real time stream process. With AWS flink job we can handle 10K requet per second. 
            The flink job also save the quiz answer to the 8.2 real time database

            5.3 **Score Event** : This real time background job will evaluate the quiz anwser and save the result to the 8.2 real-time database. This job also query and calcualte the user's core and update the leaderboard.

            5.4 **Leaderboad Service** : Provide an API for user to get the realtime leaderboard information.

            5.5 **Notification serice** : This realtime job will forward the message to 3.Google firebase and then the google firebase will forward the message to the client.

            5.6 **Quiz service**: Provide a list api to help user get the list of quizs, join a quiz sessions with session Id and get a quiz result.

            5.7 **Quiz Event** : This near rereal time job is to sync the quiz result, user's score and leaderboard to the master data. These information.

        6. **Bus & Event** : Contains the message bus support queue and pub/sub pattern

            6.1 **Stream Result** : This SNS holds the notification of streaming flink job. After completing saving the quiz anwser to the 8.2 real time database, The flink job will rase a stream result event.

            6.2 **ui-response**: This SQS queue holds the notification message which will be delivered to the client.

            6.3 **score-event**: This SQS holds the score event message. This queue will trigger the score-event background job to calculate the user's core and update the leaderboard.

            6.4 **quiz-result**: This SQS holds the quiz result event. After the score-event complete the work, it will raise a message to this queue to trigger the quiz-event background job sync the user's quiz result and user's core, user's rank to the master database.

        7. **Real time treaming process** : To handle a large number of users or quiz sesion. I decided to use the streaming event to handle the request.

            7.1 **Stream Event**: This AWS Event Bridge will delivery the request from quiz-service or core-service to the kinesis stream.

            7.2 **Kinesis stream** : This streaming data can handle 10k request per second or more depend on the configuration and the size of instance.

            7.3 **AWS flink job**: This apache flink job will process the stream event from Kinesis stream. After receive the quiz answer, the fink job will do the validation and save the data to the 8.2 real-time datbase.

        8. **Data storage**: Including the master data, realtime data and caching data. 

            8.1 **Master data**: This AWS Aurora database (PosgreSQL) store the master data of the application including the quizs, users, settings...etc.

            8.2 **Real time database** : AWS dynamoDB stores the real time user's anwser for each quiz, user's score, and leaderboard.

            8.3 **Cache data** : AWS elastic cache store the cache data to boost performance and reduce the redudant hit to master database.
        
        9. **API gateway** : AWS api gateway handle API traffic, enforcing security policies, implementing traffic management rules, and facilitating integration with backend services.


   - **Data Flow**: 
   ![Fulld data flow](/Architecture/ELSA-Data-Flow.drawio.svg)

   - **Technologies and Tools**: ![Logic technical model](/Architecture/ELSA-Technology-and-Tools.drawio.svg)
        -   auth0.com is the authentication and authorization provider for the whole system
        - All applications run on AWS cloud.
        -  All microservices are in the EKS cluster. The kubernetes will help us easily scale up the system to server the large traffics.
        - For storage: We use the Aurora Posgresql for master data and user information. The AWS elastic cache is the power cache provider to boost application performance. For real time database I decided to use the AWS dynamo DB.

        - For monitoring and observability: I decided to use the datadog. The good platform for mornitoring the whole application including the infrastructure, application logs and error tracking.

        - For streaming process: I decided to use the AWS Kinedis stream. This service can handle a large number of quiz sessions or users.

        - With Kinesis, dynamo db, and EKS, we have a power platform to easily scale horizontally to serve the huge traffics in peak hours with the reasonable cost. Because these services are able to scale up or scale down depends on the traffics.

        - Reliability : The AWS global infrastructure is built around  AWS regions and availability zones. So it's easily to spin yp  the new service to replace the unhealthy services.
        - To manage the infrastructure of the whole project, I decided to use the terraform project.