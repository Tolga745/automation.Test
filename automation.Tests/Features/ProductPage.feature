Feature: Product page feature


Background:
    Given I am on "BASE_URL"
    When   I click the sign in link
        And   I enter email as "ADMIN_EMAIL"
        And   I enter the password as "ADMIN_PASSWORD"
        And   I press the sign in button
    Then  I should be on "BASE_URL"

Scenario: Add a product to the wishlist and check the wishlist page
    
    When I go to "MEN_URL"
        And I click the 1 st product
        And I selected the size as "S"
        And I selected the color as 1
        And I press the add to wishlist button
    When I enter the wishlist page
    Then I should be on "WISHLIST_URL"
    And I should see the added product in my wishlist

Scenario Outline: Products are correctly ordered by price
    When I go to "MEN_URL"
    And I order the products by price "<sortDirection>"
    Then the products should be ordered by price "<sortDirection>"

Examples:
    | sortDirection |
    | low to high   |
    | high to low   |