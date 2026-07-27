# Plan: Remove Inline Styles & Add SCSS for Navbar + Auth Pages

## Context
- `frontend/src/styles.scss` defines the global design tokens (pink primary theme, radii, typography, etc.).
- `frontend/src/app/presentation/components/shared/navbar/navbar.component.html` has heavy inline styling using indigo `#3f51b5`.
- Auth pages (`login`, `register`, `forgot-password`, `reset-password`, `verify-email`) use shared class names (`auth-container`, `auth-card`, `btn-primary`, `divider`, `auth-link`, `form-group`, `alert`, `spinner`, `subtitle`) but none of these are defined in any SCSS file.
- `forgot-password` and `reset-password` HTML files contain inline styles for header icons and footers.
- Navbar SCSS is empty.

## Decisions
1. **Keep existing indigo brand color for navbar/auth pages.** Do not force pink onto these components. The navbar brand color `#3f51b5` and auth icon background `#e8eaf6` remain as-is.
2. **Shared auth classes go into `styles.scss`** as global styles because they are reused across every auth page.
3. **Page-specific / component-specific styles** go into each component’s own `.scss` file.
4. **All inline styles must be removed** and replaced with class names.

## Task List

### 1. Add global auth styles to `frontend/src/styles.scss`
Append after existing content:
- `.auth-container` — full-width centered flex container
- `.auth-card` — white card, rounded corners, shadow, max-width ~420px, padding
- `.auth-card.text-center` — center text
- `.mb-lg` — margin-bottom spacing utility
- `.form-group` — label, input, error-text vertical stack
- `.form-group label` — label styling (font-weight, margin-bottom)
- `.form-group input` — input styling (border, radius, padding, focus ring)
- `.error-text` — error message styling (color: destructive, small text)
- `.btn-primary` — primary button (uses `--primary` / `--ring` tokens)
- `.btn-primary:disabled` — disabled state
- `.btn-google` — Google button (white bg, border, flex row)
- `.spinner` — loading spinner animation
- `.divider` — horizontal rule with centered "OR" text
- `.auth-link` — link styling (color, hover, no underline)
- `.alert` — alert container (padding, border-radius, border-left)
- `.alert-error` — error alert
- `.alert-success` — success alert
- `.subtitle` — muted text utility
- `.text-center` — text-align center

### 2. Update `navbar.component.html`
Remove all inline `style=` attributes and replace with CSS classes:
- `header` → keep class `top-header`
- `h3` → add class `brand-name`
- divider → add class `header-divider`
- `search-box` → add class `search-box`
- search input → add class `search-input`
- search icon span → add class `search-icon`
- bell button → add class `icon-btn notification-btn`
- badge span → add class `notification-badge`
- settings button → add class `icon-btn`
- user info div → add class `user-info`
- avatar div → add class `user-avatar`
- username span → add class `user-name`
- logout button → add class `logout-btn`

### 3. Update `navbar.component.scss`
Add styles for all navbar classes. Use `#3f51b5` for brand elements, `#666` for muted text, `#ddd` for dividers. Ensure flex layout, spacing, and responsive behavior.

### 4. Update `login.scss`
Add styles for:
- `.login-card` — text-align center, margin-bottom
- `.login-icon` — 56px circle, `#e8eaf6` background, centered flex
- `.password-field` — cursor pointer (already exists)

### 5. Update `forgot-password.component.html`
Remove inline styles from:
- header wrapper div → add class `auth-header`
- icon wrapper div → add class `auth-icon`
- footer wrapper div → add class `auth-footer`
- back-link anchor → add class `auth-link auth-back-link`

### 6. Update `forgot-password.component.scss`
Add styles for `.auth-header`, `.auth-icon`, `.auth-footer`, `.auth-back-link`.

### 7. Update `reset-password.component.html`
Same pattern as forgot-password:
- header wrapper → `.auth-header`
- icon wrapper → `.auth-icon`
- footer wrapper → `.auth-footer`

### 8. Update `reset-password.component.scss`
Add styles for `.auth-header`, `.auth-icon`, `.auth-footer`.

### 9. `verify-email` pages
No inline styles. Keep `.verify-icon`, `.verify-actions`, `.verify-footer` in `verify-email.component.scss` as-is. Global auth classes from `styles.scss` will cover the rest.

### 10. `register` page
No inline styles. Keep existing class names. Global auth classes from `styles.scss` will cover the rest.

## Validation
- Run Angular build / lint to ensure no template parse errors from removed inline styles.
- Verify all auth pages render correctly with the new global classes.
- Verify navbar renders with new SCSS classes.

## Open Questions
None.
