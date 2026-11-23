Feature: Cart page feature

Scenario: Guest cart items remain after sign in
    Given I am on "BASE_URL"
    When I go to "MEN_URL"
        And I click the 1 st product
        And I selected the size as "S"
        And I selected the color as 1
        And I press the add to cart button
    When I click the sign in link
        And I enter email as "ADMIN_EMAIL"
        And I enter the password as "ADMIN_PASSWORD"
        And I press the sign in button
        And I enter the cart
    Then I should see the guest cart product in the cart