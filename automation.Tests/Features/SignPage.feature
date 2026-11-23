Feature: Sign in and Sign Up

Scenario: User can sign in with a valid account
    Given I am on "BASE_URL"
    When  I click the sign in link
    And   I enter email as "ADMIN_EMAIL"
    And   I enter the password as "ADMIN_PASSWORD"
    And   I press the sign in button
    Then  I should be on "BASE_URL"


Scenario: User can sign up, then sign in with the account
    Given I am on "BASE_URL"
    When I click the sign up link
    And  I enter email as "NEW_USER_EMAIL"
    And  I enter the password as "NEW_USER_PASSWORD"
    And I enter the password confirmation as "NEW_USER_PASSWORD"
    And I press the sign up button
    Then I should be on "SIGNIN_URL"
    When  I enter email as "NEW_USER_EMAIL"
    And  I enter the password as "NEW_USER_PASSWORD"
    And  I press the sign in button
    Then I should be on "BASE_URL"

Scenario: User cannot sign in with an unregistered account
    Given I am on "BASE_URL"
    When  I click the sign in link
    And   I enter email as "NEW_USER_EMAIL"
    And   I enter the password as "NEW_USER_PASSWORD"
    And   I press the sign in button
    Then  I should see an error message "No account found with this email address."

Scenario: User cannot sign up with non matching password and confirmation
    Given I am on "BASE_URL"
    When I click the sign up link
    And  I enter email as "NEW_USER_EMAIL"
    And  I enter the password as "NEW_USER_PASSWORD"
    And I enter the password confirmation as "NEW_USER_MISMATCH_PASSWORD"
    And I press the sign up button
    Then I should see an error message "Passwords don't match."

Scenario Outline: User cannot sign up with a badly formatted email
    Given I am on "BASE_URL"
    When I click the sign up link
    And  I enter email as "<email>"
    And  I enter the password as "NEW_USER_PASSWORD"
    And I enter the password confirmation as "NEW_USER_PASSWORD"
    And I press the sign up button
    Then I should see an error message "Invalid email format."

    Examples:
      | email               |
      | testexample.com     |
      | test@.com           |
      | @example.com        |
      | test@example        |

Scenario Outline: Check different special characters in the password
    Given I am on "BASE_URL"
    When I click the sign up link
    And  I enter email as "NEW_USER_EMAIL"
    And  I enter the password as "<passwordKey>"
    And I enter the password confirmation as "<passwordKey>"
    And I press the sign up button
    Then I should be on "SIGNIN_URL"
    Examples:
      | passwordKey             |
      | INVALID_PASSWORD_DOT    |
      | VALID_PASSWORD          |
      | INVALID_PASSWORD_COMMA  |
      | VALID_PASSWORD_DOLLAR   |
      | VALID_PASSWORD_PERCENT  |
      | INVALID_PASSWORD_CARET  |
      | VALID_PASSWORD_AMPERSAND|


Scenario: System can handle spaces in the email
    Given I am on "BASE_URL"
    When I click the sign up link
    And  I enter email as "NEW_USER_EMAIL"
    And  I enter the password as "NEW_USER_PASSWORD"
    And I enter the password confirmation as "NEW_USER_PASSWORD"
    And I press the sign up button
    Then I should be on "SIGNIN_URL"