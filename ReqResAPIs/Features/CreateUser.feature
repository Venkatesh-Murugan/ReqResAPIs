Feature: Create Users api automation tests
As an automation engineer, I need to create new user and test the create user responses

Scenario: Create a user
	Given I create a request payload for users as below
	| name   | job        |
	| venkat | contractor |
	| karunN | employee   |
	When I send a POST request to the users endpoint
	Then the status code should be 201
	And the response should have the new user details 

Scenario: Create a user only by name
	Given I create a request payload for users as below
	| name         |
	| venkateshyem |
	When I send a POST request to the users endpoint
	Then the status code should be 201
	And the job value should be null
