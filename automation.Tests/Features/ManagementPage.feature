Feature: Managemrent page feature

Scenario: Customer cant access the product management page
    Given I am on "BASE_URL"
    When   I click the sign in link
        And   I enter email as "CUSTOMER_EMAIL" 
        And   I enter the password as "CUSTOMER_PASSWORD"
        And   I press the sign in button
    Then  I should be on "BASE_URL"
    When I go to "PRODUCT_MANAGER_URL"
    Then I should be on "UNAUTHORIZED_URL"

    
    Scenario: Customer cant access the sales management page
    Given I am on "BASE_URL"
    When   I click the sign in link
    And   I enter email as "CUSTOMER_EMAIL" 
    And   I enter the password as "CUSTOMER_PASSWORD"
    And   I press the sign in button
    Then  I should be on "BASE_URL"
    When I go to "SALES_MANAGER_URL"
    Then I should be on "UNAUTHORIZED_URL"

    @edge
    Scenario: Over hundred percent discount is not allowed
    Given I am on "BASE_URL"
    When   I click the sign in link
        And   I enter email as "ADMIN_EMAIL"
        And   I enter the password as "ADMIN_PASSWORD"
        And   I press the sign in button
    Then  I should be on "BASE_URL"
    When I go to "SALES_MANAGER_URL"
        And I click the price & discount button
        And I select the first product
        And I enter the discount as "INVALID_DISCOUNT"
        And I press the update discount button
    Then I should see a message "Please enter a discount between 0 and 100"