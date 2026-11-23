Feature: Checkout feature
Background:
    Given I am on "BASE_URL"
    When   I click the sign in link
    And   I enter email as "ADMIN_EMAIL"
    And   I enter the password as "ADMIN_PASSWORD"
    And   I press the sign in button
    Then  I should be on "BASE_URL"

Scenario: Add a product to the cart and check out
    
    When I go to "MEN_URL"
    And I click the 1 st product
    And I selected the size as "S"
    And I selected the color as 1
    And I press the add to cart button
    When I enter the cart
    And I press the order now button
    Then I should be on "PAYMENT_URL"
    When I enter the address as "TEST_ADDRESS"
    And I enter the card number as "TEST_CARD_NUMBER"
    And I enter the expiration date as "TEST_CARD_EXPIRATION"
    And I enter the CVV as "TEST_CARD_CVV"
    And I press the make payment button
    Then I should be on "CONFIRMATION_URL"
    And I should see the order confirmation message "Order Confirmed!"
    And I should see the invoice preview

Scenario: Try to go directly to the checkout page without adding a product to the cart
    
    When I enter the cart
    And I press the order now button
    Then I should be on "CART_URL"
    And I should see a message "Your cart is empty."

Scenario: Try check out with empty address
When I go to "MEN_URL"
    And I click the 1 st product
    And I selected the size as "S"
    And I selected the color as 1
    And I press the add to cart button
    And I enter the cart
    And I press the order now button
    Then I should be on "PAYMENT_URL"
    When I enter the address as ""
    And I enter the card number as "TEST_CARD_NUMBER"
    And I enter the expiration date as "TEST_CARD_EXPIRATION"
    And I enter the CVV as "TEST_CARD_CVV"
    And I press the make payment button
    Then I should see a message "Please fill out this field."
