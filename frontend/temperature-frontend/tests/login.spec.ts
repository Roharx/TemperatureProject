import { test, expect, Page } from '@playwright/test';

// Helper function to obtain a valid token
test.use({ headless: true });

test.describe('LoginComponent Tests', () => {
  let validToken: string;

  test.beforeAll(async ({ browser }) => {
    const page = await browser.newPage();
    await page.close();
  });

  test.beforeEach(async ({ page }) => {
    // Navigate to your application's login page
    await page.goto('http://localhost:4200');
    console.log('Navigated to login page');

    // Insert the valid token into local storage
    await page.evaluate(token => {
      localStorage.setItem('auth_token', token);
    }, validToken);
  });

  test('should login successfully', async ({ page }) => {
    await page.goto('http://localhost:4200/login');
    await page.fill('input#username', 'asd');
    await page.fill('input#password', 'asdqwe');
    await page.press('input#password', 'Enter');

    // Navigate to main screen
    await page.goto('http://localhost:4200/main-screen');
  });

  test('should show validation messages when fields are invalid', async ({ page }) => {
    // Initially, the submit button should be disabled
    const loginButton = page.locator('button[type="submit"]');
    await expect(loginButton).toBeDisabled();

    // Fill in an invalid username
    await page.fill('input#username', 'a');
    await expect(loginButton).toBeDisabled();

    // Fill in an invalid password
    await page.fill('input#password', 'short');
    await expect(loginButton).toBeDisabled();

    // Verify the error message for short password
    await expect(page.locator('input#password + .error-message')).toContainText('Password must be at least 6 characters long.');

    // Complete the username field to make it valid
    await page.fill('input#username', 'validuser');
    await expect(loginButton).toBeDisabled();

    // Complete the password field to make it valid
    await page.fill('input#password', 'validpassword');
    await expect(loginButton).toBeEnabled();
  });

  test('should show validation message on short password', async ({ page }) => {
    await page.fill('input#username', 'testuser');
    await page.fill('input#password', 'short');

    // Ensure the form is invalid and button is disabled
    const loginButton = page.locator('button[type="submit"]');
    await expect(loginButton).toBeDisabled();

    // Verify the error message for short password
    await expect(page.locator('input#password + .error-message')).toContainText('Password must be at least 6 characters long.');

    // Complete the password field to make it valid
    await page.fill('input#password', 'validpassword');
    await expect(loginButton).toBeEnabled();
  });

  test('should switch to register mode', async ({ page }) => {
    await page.click('text="Don\'t have an account? Register"');
    await expect(page.locator('form')).toContainText('Name');
    await expect(page.locator('form')).toContainText('Email');
    await expect(page.locator('form')).toContainText('Password');
    await expect(page.locator('button[type="submit"]')).toContainText('Register');
  });

  test('should register successfully', async ({ page }) => {
    // Mock the register API response
    await page.route('**/api/Account/register', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ responseData: 'registered' }),
      });
    });

    // Switch to register mode
    await page.click('text="Don\'t have an account? Register"');

    // Fill in the registration form
    await page.fill('input#name', 'Test User');
    await page.fill('input#email', 'test@example.com');
    await page.fill('input#password', 'validpassword');

    // Ensure the form is valid and button is enabled
    const registerButton = page.locator('button[type="submit"]');
    await expect(registerButton).toBeEnabled();

    await registerButton.click();

    // Expect to see a success message
    const toastMessage = page.locator('.toast-message');
    await expect(toastMessage).toContainText('Registration successful! You can now log in.');
  });

});
