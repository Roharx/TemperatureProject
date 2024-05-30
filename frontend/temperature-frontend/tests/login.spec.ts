import { test, expect } from '@playwright/test';

const BASE_URL = 'http://161.97.92.174';

test.use({ headless: true });
test.describe('LoginComponent Tests', () => {
  let validToken: string;

  test.beforeAll(async ({ browser }) => {
    const page = await browser.newPage();
    await page.close();
  });

  test.beforeEach(async ({ page }) => {
    // Navigate to your application's login page
    await page.goto(BASE_URL);
    console.log('Navigated to login page');

    // Insert the valid token into local storage
    await page.evaluate(token => {
      localStorage.setItem('auth_token', token);
    }, validToken);
  });

  test('should login successfully', async ({ page }) => {
    test.setTimeout(60000); // Increase timeout to 60 seconds

    await page.goto(`${BASE_URL}`);
    console.log('Navigated to login page');
    await page.fill('input#username', 'asd');
    console.log('Filled username');
    await page.fill('input#password', 'asdqwe');
    console.log('Filled password');
    await page.press('input#password', 'Enter');
    console.log('Submitted login form');

    // Wait for the main screen to load
    await page.waitForSelector('text=Logout');
    console.log('Navigated to main screen');
  });

  test('should show validation messages when fields are invalid', async ({ page }) => {
    test.setTimeout(60000); // Increase timeout to 60 seconds

    await page.goto(`${BASE_URL}`);
    console.log('Navigated to login page');

    // Initially, the submit button should be disabled
    const loginButton = page.locator('button[type="submit"]');
    await expect(loginButton).toBeDisabled();
    console.log('Login button is disabled');

    // Fill in an invalid username
    await page.fill('input#username', 'a');
    await expect(loginButton).toBeDisabled();
    console.log('Invalid username filled, login button still disabled');

    // Fill in an invalid password
    await page.fill('input#password', 'short');
    await expect(loginButton).toBeDisabled();
    console.log('Invalid password filled, login button still disabled');

    // Verify the error message for short password
    await expect(page.locator('input#password + .error-message')).toContainText('Password must be at least 6 characters long.');
    console.log('Error message for short password is displayed');

    // Complete the username field to make it valid
    await page.fill('input#username', 'validuser');
    await expect(loginButton).toBeDisabled();
    console.log('Valid username filled, login button still disabled');

    // Complete the password field to make it valid
    await page.fill('input#password', 'validpassword');
    await expect(loginButton).toBeEnabled();
    console.log('Valid password filled, login button enabled');
  });

  test('should show validation message on short password', async ({ page }) => {
    test.setTimeout(60000); // Increase timeout to 60 seconds

    await page.goto(`${BASE_URL}`);
    console.log('Navigated to login page');

    await page.fill('input#username', 'testuser');
    await page.fill('input#password', 'short');

    // Ensure the form is invalid and button is disabled
    const loginButton = page.locator('button[type="submit"]');
    await expect(loginButton).toBeDisabled();
    console.log('Invalid form, login button disabled');

    // Verify the error message for short password
    await expect(page.locator('input#password + .error-message')).toContainText('Password must be at least 6 characters long.');
    console.log('Error message for short password is displayed');

    // Complete the password field to make it valid
    await page.fill('input#password', 'validpassword');
    await expect(loginButton).toBeEnabled();
    console.log('Valid password filled, login button enabled');
  });

  test('should switch to register mode', async ({ page }) => {
    test.setTimeout(60000); // Increase timeout to 60 seconds

    await page.goto(`${BASE_URL}`);
    console.log('Navigated to login page');

    await page.click('text="Don\'t have an account? Register"');
    console.log('Clicked switch to register mode');

    await expect(page.locator('form')).toContainText('Name');
    await expect(page.locator('form')).toContainText('Email');
    await expect(page.locator('form')).toContainText('Password');
    await expect(page.locator('button[type="submit"]')).toContainText('Register');
    console.log('Switched to register mode');
  });

  test('should register successfully', async ({ page }) => {
    test.setTimeout(60000); // Increase timeout to 60 seconds

    await page.goto(`${BASE_URL}`);
    console.log('Navigated to login page');

    // Mock the register API response
    await page.route('**/api/Account/register', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ responseData: 'registered' }),
      });
    });
    console.log('Mocked register API response');

    // Switch to register mode
    await page.click('text="Don\'t have an account? Register"');
    console.log('Clicked switch to register mode');

    // Fill in the registration form
    await page.fill('input#name', 'Test User');
    await page.fill('input#email', 'test@example.com');
    await page.fill('input#password', 'validpassword');
    console.log('Filled registration form');

    // Ensure the form is valid and button is enabled
    const registerButton = page.locator('button[type="submit"]');
    await expect(registerButton).toBeEnabled();

    await registerButton.click();
    console.log('Submitted registration form');

    // Expect to see a success message
    const toastMessage = page.locator('.toast-message');
    await expect(toastMessage).toContainText('Registration successful! You can now log in.');
    console.log('Registration successful');
  });
});
