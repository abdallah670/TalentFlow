---
name: TalentFlow Enterprise
colors:
  surface: '#fbf8ff'
  surface-dim: '#dbd9e2'
  surface-bright: '#fbf8ff'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f4f2fc'
  surface-container: '#efedf6'
  surface-container-high: '#e9e7f0'
  surface-container-highest: '#e3e1ea'
  on-surface: '#1a1b22'
  on-surface-variant: '#454652'
  inverse-surface: '#2f3037'
  inverse-on-surface: '#f2eff9'
  outline: '#757684'
  outline-variant: '#c5c5d4'
  surface-tint: '#4355b9'
  primary: '#24389c'
  on-primary: '#ffffff'
  primary-container: '#3f51b5'
  on-primary-container: '#cacfff'
  inverse-primary: '#bac3ff'
  secondary: '#565c84'
  on-secondary: '#ffffff'
  secondary-container: '#c9cffd'
  on-secondary-container: '#51577f'
  tertiary: '#6c3400'
  on-tertiary: '#ffffff'
  tertiary-container: '#8f4700'
  on-tertiary-container: '#ffc7a2'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#dee0ff'
  primary-fixed-dim: '#bac3ff'
  on-primary-fixed: '#00105c'
  on-primary-fixed-variant: '#293ca0'
  secondary-fixed: '#dee0ff'
  secondary-fixed-dim: '#bec4f2'
  on-secondary-fixed: '#12183d'
  on-secondary-fixed-variant: '#3e446b'
  tertiary-fixed: '#ffdcc6'
  tertiary-fixed-dim: '#ffb784'
  on-tertiary-fixed: '#301400'
  on-tertiary-fixed-variant: '#713700'
  background: '#fbf8ff'
  on-background: '#1a1b22'
  surface-variant: '#e3e1ea'
typography:
  h1:
    fontFamily: Roboto
    fontSize: 28px
    fontWeight: '500'
    lineHeight: 36px
    letterSpacing: -0.01em
  h2:
    fontFamily: Roboto
    fontSize: 22px
    fontWeight: '500'
    lineHeight: 28px
    letterSpacing: '0'
  h3:
    fontFamily: Roboto
    fontSize: 18px
    fontWeight: '500'
    lineHeight: 24px
    letterSpacing: '0'
  body:
    fontFamily: Roboto
    fontSize: 15px
    fontWeight: '400'
    lineHeight: 22px
    letterSpacing: '0'
  body-sm:
    fontFamily: Roboto
    fontSize: 13px
    fontWeight: '400'
    lineHeight: 18px
    letterSpacing: '0'
  label:
    fontFamily: Roboto
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.02em
  h1-mobile:
    fontFamily: Roboto
    fontSize: 24px
    fontWeight: '500'
    lineHeight: 32px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  unit: 4px
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 32px
  gutter: 24px
  margin: 24px
---

## Brand & Style
The design system is engineered for high-performance HR environments, focusing on clarity, trust, and rhythmic efficiency. It adopts a **Modern Corporate** aesthetic that prioritizes information density without sacrificing legibility. 

The visual narrative is "Reliable Efficiency." By utilizing a structured grid and a disciplined color application, the design system ensures that recruiters can process large volumes of applicant data with minimal cognitive load. The interface feels established and institutional, yet retains a contemporary edge through the use of purposeful accent colors and ample whitespace.

## Colors
The palette is anchored by deep indigo tones to project authority and stability. 
- **Primary Dark** is reserved for high-level navigation and structural headers to ground the application.
- **Primary** serves as the main action color, used for critical paths and interactive states.
- **Accent (Orange)** is used sparingly for Call-to-Action elements and high-priority notifications to ensure they break through the cool-toned interface.
- **Semantic Colors** (Success, Warning, Error) follow industry standards to provide immediate, intuitive feedback on application statuses and system alerts.

## Typography
This design system utilizes **Roboto** for its systematic, geometric, yet approachable character. 
- **Headlines (H1-H3):** Use a medium weight (500) to provide clear hierarchy in data-heavy screens. 
- **Body Text:** Set at 15px to optimize for long-form reading of resumes and notes.
- **Labels:** Utilized for metadata and small captions, often employing a slightly increased letter spacing and uppercase styling for distinct separation from body content.

## Layout & Spacing
The layout follows a **12-column fixed grid** on desktop (max-width 1440px) to maintain a consistent reading line. On smaller screens, the system transitions to a fluid grid.
- **Desktop:** 24px margins and gutters.
- **Tablet:** 16px margins and gutters.
- **Mobile:** 16px margins with a single-column stack.

Spacing is based on a **4px base unit**, ensuring that all components (buttons, inputs, margins) align to a consistent mathematical rhythm.

## Elevation & Depth
Depth is conveyed through **Tonal Layers** and subtle **Ambient Shadows**. 
- The background sits at the lowest level (`#F5F6FA`).
- Cards and Panels sit on the surface (`#FFFFFF`) with a very soft shadow: `0 1px 3px rgba(0,0,0,0.12)`.
- Interactive elements like dropdowns or modals use a higher elevation shadow (`0 4px 12px rgba(0,0,0,0.15)`) to indicate they are temporary overlays.
- Border definitions (`#E0E0E0`) are used to separate sections within the same surface tier.

## Shapes
The shape language is primarily **Soft**, using a 4px-8px radius for most structural elements to balance professional rigidity with modern friendliness.
- **Standard UI (Buttons/Inputs):** 4px radius for a precise, "tool-like" feel.
- **Containers (Cards/Modals):** 8px radius to soften the larger blocks of color.
- **Indicators (Chips):** Full pill-shape (16px+) to differentiate status tags from interactive buttons.

## Components
### Buttons & Inputs
- **Primary Button:** Solid `#3F51B5` with white text, 4px radius.
- **Secondary Button:** Outline of `#3F51B5` or text-only for less emphasis.
- **Input Fields:** White background with a 1px `#E0E0E0` border. Active state shifts border to Primary.

### Job Status Badges (Chips)
Status chips are pill-shaped and use high-contrast text on a light background for visibility:
- **Open:** Green text on Light Green background.
- **Closed:** Secondary Text on Light Gray background.
- **On Hold:** Warning Orange text on Light Orange background.
- **Urgent:** White text on Accent Orange (`#FF6D00`) background.

### KPI Cards
KPI cards are specialized surface elements used at the top of dashboards:
- **Layout:** Large H2 for the metric, small Label for the description.
- **Visuals:** May include a small sparkline chart or a percentage trend indicator in Success/Error colors.

### Lists & Tables
- **Rows:** 56px minimum height. 
- **Hover State:** Background shifts to `#C5CAE9` (Primary Light) with a 20% opacity for clear row tracking.