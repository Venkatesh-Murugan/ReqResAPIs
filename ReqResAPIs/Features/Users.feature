Feature: Users api automation tests
As an automation engineer, I need to test the users api

Scenario: Retrieve list of all users
	When I send a GET request to '/users'
	Then the response should have a status code of 200
	And the response body should contain a list of users

Scenario: Retrieve a list of users with invalid id parameter
	When I send a GET request to '/users' with id as 33
	Then the response should have a status code of 404
	And the response data should be empty

	Scenario: Single user
	When I send a GET request to '/users' with id as 3
	Then the response should have a status code of 200
	And the response data should be based on the filter criteria applied
