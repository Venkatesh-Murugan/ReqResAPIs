Feature: Users api automation tests
As an automation engineer, I need to test the users api

Scenario: Retrieve list of all users
	When I send a GET request to '/users'
	Then the response should have a status code of 200
	And the response body should contain a list of users

Scenario: Retrieve a list of users with invalid parameters
	When I send a GET request to '/users' with invalid page as 3
	Then the response should have a status code of 200
	And the response body should contain a filtered list of users with the specified parameters
