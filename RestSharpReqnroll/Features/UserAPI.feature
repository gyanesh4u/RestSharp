Feature: User API Operations
  As a tester
  I want to test User API endpoints
  So that I can ensure the API works correctly

  Background:
    Given I have the API base URL "https://jsonplaceholder.typicode.com"

  @smoke
  Scenario: Get a user by ID
    When I get user with ID "1"
    Then the response status should be success
    And the response should contain user data

  @smoke
  Scenario: Get all users
    When I get all users
    Then the response status should be success
    And the response should contain a list of users

  Scenario: Verify user details
    When I get user with ID "1"
    Then the user should have name "Leanne Graham"
    And the user should have email "Sincere@april.biz"
