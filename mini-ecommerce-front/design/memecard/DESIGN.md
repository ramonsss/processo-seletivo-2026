---
name: MemeCard
colors:
  surface: '#14121f'
  surface-dim: '#14121f'
  surface-bright: '#3a3746'
  surface-container-lowest: '#0e0c19'
  surface-container-low: '#1c1a27'
  surface-container: '#201e2c'
  surface-container-high: '#2b2836'
  surface-container-highest: '#363342'
  on-surface: '#e5e0f3'
  on-surface-variant: '#ccc3d8'
  inverse-surface: '#e5e0f3'
  inverse-on-surface: '#312f3d'
  outline: '#958da1'
  outline-variant: '#4a4455'
  surface-tint: '#d2bbff'
  primary: '#d2bbff'
  on-primary: '#3f008e'
  primary-container: '#7c3aed'
  on-primary-container: '#ede0ff'
  inverse-primary: '#732ee4'
  secondary: '#ffc640'
  on-secondary: '#402d00'
  secondary-container: '#e3aa00'
  on-secondary-container: '#5a4100'
  tertiary: '#ddb7ff'
  on-tertiary: '#490080'
  tertiary-container: '#8d36db'
  on-tertiary-container: '#f2dfff'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#eaddff'
  primary-fixed-dim: '#d2bbff'
  on-primary-fixed: '#25005a'
  on-primary-fixed-variant: '#5a00c6'
  secondary-fixed: '#ffdf9f'
  secondary-fixed-dim: '#f9bd22'
  on-secondary-fixed: '#261a00'
  on-secondary-fixed-variant: '#5c4300'
  tertiary-fixed: '#f0dbff'
  tertiary-fixed-dim: '#ddb7ff'
  on-tertiary-fixed: '#2c0051'
  on-tertiary-fixed-variant: '#6900b3'
  background: '#14121f'
  on-background: '#e5e0f3'
  surface-variant: '#363342'
typography:
  display-lg:
    fontFamily: Montserrat
    fontSize: 48px
    fontWeight: '800'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Montserrat
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
  headline-lg-mobile:
    fontFamily: Montserrat
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 32px
  headline-md:
    fontFamily: Montserrat
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  label-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
    letterSpacing: 0.05em
  stats-number:
    fontFamily: Montserrat
    fontSize: 20px
    fontWeight: '700'
    lineHeight: 24px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  base: 4px
  xs: 0.5rem
  sm: 1rem
  md: 1.5rem
  lg: 2rem
  xl: 4rem
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 64px
---

## Brand & Style
The design system is engineered for a high-energy, digital-first trading card game platform tailored to the Brazilian meme culture. The brand personality is **irreverent yet premium**, blending the chaotic energy of internet memes with the sophisticated mechanics of a fintech or high-end gaming dashboard.

The aesthetic follows a **Modern Minimalism** approach with **Glassmorphic** accents. It utilizes deep blacks and vibrant purples to create a "Neon Noir" atmosphere. Subtle grain textures and micro-grids are used to provide a tactile, analog feel to digital assets, ensuring the platform feels like a curated gallery rather than a cluttered forum. The emotional response should be one of "exclusive hype"—making every meme card feel like a valuable collector's item.

## Colors
The palette is dominated by **Primary Deep Space (#0F0D1A)** to ensure that card artwork and rarity glows remain the focal point. 

- **Primary Accent (Electric Purple):** Used for primary CTAs, active states, and general branding.
- **SSR Rarity (Gold):** Reserved strictly for Super Super Rare items, high-value legendary cards, and premium transaction buttons.
- **SR Rarity (Muted Purple):** Used for Super Rare items and secondary highlights.
- **Neutral Scales:** Grays are tinted with purple (e.g., #1A162E) to maintain depth and avoid a "flat" black appearance.

## Typography
The system uses a pairing of **Montserrat** for impactful, bold headlines and **Inter** for highly legible UI elements and body text.

- **Headlines:** Use Montserrat with heavy weights (700+) to convey strength and urgency.
- **Body:** Inter provides a clean, neutral balance to the aggressive headlines.
- **Stats & Rarity:** Use Montserrat Bold for numerical values (e.g., card power levels, prices) to ensure they feel "heavy" and significant.
- **Labels:** Small caps and increased letter spacing are applied to labels to create a technical, "trading deck" feel.

## Layout & Spacing
The layout follows a **Fluid Grid** model with a 12-column structure for desktop and a 4-column structure for mobile. 

- **Grid:** On desktop, use a 12-column grid with 24px gutters. Content should be centered with a max-width of 1440px.
- **Rhythm:** Spacing follows a 4px baseline. Most components should utilize `md` (24px) padding to maintain a "breathable" and clean minimalist look.
- **Card Containers:** Use an aspect ratio of 2:3 for trading cards. In the marketplace view, cards should reflow from 5 columns (desktop) to 2 columns (mobile).

## Elevation & Depth
Depth is achieved through **Tonal Layers** and **Glow Effects** rather than traditional drop shadows.

- **Background:** The base layer is the deepest neutral (#0F0D1A).
- **Surface Layer:** Secondary surfaces (cards, sidebars) use a slightly lighter purple-tinted black (#1A162E) with a subtle 1px border (opacity 10%).
- **Luminosity:** Rarity is communicated via "Backdrop Glows." SSR cards feature a soft gold outer glow (`box-shadow: 0 0 20px rgba(251, 191, 36, 0.3)`), while SR cards use a purple glow.
- **Glassmorphism:** Navigation bars and modal overlays should use a background blur (12px) with a 20% opacity fill of the primary accent color.

## Shapes
The design system utilizes **Soft (0.25rem)** roundedness to maintain a precise, modern, and slightly aggressive tech aesthetic. 

- **Standard Elements:** Buttons and input fields use 4px (`0.25rem`) corners.
- **Trading Cards:** Cards use `rounded-lg` (8px) to feel substantial and physical.
- **Icons:** Use linear, 2px stroke icons with sharp terminals to match the font geometry.

## Components
- **Buttons:**
    - *Primary:* Solid Electric Purple (#7C3AED) with white text. High contrast, no border.
    - *Secondary:* Transparent with a 1px Purple border.
    - *SSR Special:* Gold gradient background with black text for high-value actions (e.g., "MINT CARD").
- **Trading Cards:** The centerpiece component. It must include a top-down light gradient, the meme image, a bottom-aligned stats bar, and a rarity indicator tag in the top-right corner.
- **Chips/Tags:** Small, pill-shaped indicators for rarity (SSR, SR, Common). SSR tags must use the gold color with a slight shimmer animation.
- **Inputs:** Dark backgrounds (#0F0D1A) with 1px borders that glow Electric Purple on focus.
- **Progress Bars:** Used for card experience levels. These should be thin (4px) with a glowing purple fill.
- **Marketplace Lists:** Use a clean list view with high-contrast price labels in Montserrat Bold.