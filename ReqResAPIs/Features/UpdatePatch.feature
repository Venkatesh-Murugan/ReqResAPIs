Feature: Update Users api automation tests
As an automation engineer, I need to update and delete user and validate the responses

Scenario: Update a user using PUT
	When I update the user with id 2 using PUT method
	Then user details should be updated
