import { test, expect } from '@playwright/test';

test.use({ headless: true });

test.describe('MainScreenComponent Tests', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to your application's login page
    await page.goto('http://localhost:4200/login');
    console.log('Navigated to login page');

    // Fill in the login form with valid credentials
    await page.fill('input#username', 'asd');
    await page.fill('input#password', 'asdqwe');
    await page.press('input#password', 'Enter');
    console.log('Submitted login form');

    // Wait for navigation to main screen
    await page.waitForURL('http://localhost:4200/main-screen', { timeout: 20000 });
    await expect(page).toHaveURL('http://localhost:4200/main-screen');
    console.log('Navigated to main screen');
  });

  test('should login and navigate to main screen successfully', async ({ page }) => {
    await expect(page).toHaveURL('http://localhost:4200/main-screen');
  });

  test('should display the sidebar menu items', async ({ page }) => {
    // Verify the presence of sidebar menu items
    await expect(page.locator('div.menu-item')).toHaveText(['Account', 'Office', 'Room', 'Logs']);
  });

  test('should display logout button and perform logout', async ({ page }) => {
    // Verify the presence of logout button
    await expect(page.locator('div.logout')).toHaveText('Logout');

    // Click the logout button and verify navigation to login page
    await page.locator('div.logout').click();
    await page.waitForURL('http://localhost:4200/login', { timeout: 20000 });
    await expect(page).toHaveURL('http://localhost:4200/login');
  });
});
