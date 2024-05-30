import { test, expect } from '@playwright/test';

const BASE_URL = 'http://161.97.92.174';

test.use({ headless: true }); // false to see what it's doing

test.describe('MainScreenComponent Tests', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to your application's login page
    await page.goto(BASE_URL);
    console.log('Navigated to login page');

    // Fill in the login form with valid credentials
    await page.fill('input#username', 'asd');
    await page.fill('input#password', 'asdqwe');
    await page.press('input#password', 'Enter');
    console.log('Submitted login form');

    // Wait for navigation to main screen
    await page.waitForURL(`${BASE_URL}/main-screen`, { timeout: 20000 });
    console.log('Navigated to main screen');
    await page.screenshot({ path: 'screenshots/navigated-to-main-screen.png' }); // Screenshot for debugging
  });

  test('should login and navigate to main screen successfully', async ({ page }) => {
    await expect(page).toHaveURL(`${BASE_URL}/main-screen`);
  });

  test('should display the sidebar menu items', async ({ page }) => {
    // Wait for the sidebar menu items to be visible
    await page.waitForSelector('div.menu-item', { timeout: 20000 });
    console.log('Sidebar menu items are visible');

    // Verify the presence of sidebar menu items
    const menuItems = await page.locator('div.menu-item');
    const texts = await menuItems.allTextContents();
    const trimmedTexts = texts.map(text => text.trim());
    expect(trimmedTexts).toEqual(['Account', 'Office', 'Room', 'Logs']);
    await page.screenshot({ path: 'screenshots/sidebar-menu-items.png' }); // Screenshot for debugging
  });

  test('should display logout button and perform logout', async ({ page }) => {
    // Wait for the logout button to be visible
    await page.waitForSelector('div.logout', { timeout: 20000 });
    console.log('Logout button is visible');

    // Verify the presence of logout button
    await expect(page.locator('div.logout')).toHaveText('Logout');

    // Click the logout button and verify navigation to login page
    await page.locator('div.logout').click();
    await page.waitForURL(`${BASE_URL}/login`, { timeout: 20000 });
    await expect(page).toHaveURL(`${BASE_URL}/login`);
    console.log('Logged out and navigated to login page');
    await page.screenshot({ path: 'screenshots/logout-successful.png' }); // Screenshot for debugging
  });
});
