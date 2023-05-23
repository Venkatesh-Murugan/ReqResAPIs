#Feature: Create Users api automation tests
#As an automation engineer, I need to update and delete user and validate the responses
#
#Scenario: Create a user
#	Given I use a request payload for '/users' as below
#	| name      | job        |
#	| venkatesh | employee   |
#	| karun     | contractor |
#	When I send a PUT request to the users endpoint
#	Then the status code should be 201
#	And the response should have the new user details
