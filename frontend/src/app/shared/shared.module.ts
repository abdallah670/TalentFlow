import { NgModule } from '@angular/core';

import { NavbarComponent } from './components/navbar/navbar.component';
import { PortalSidebarComponent } from './components/portal-sidebar/portal-sidebar.component';
import { SidebarComponent as CandidateSidebarComponent } from './components/candidate-sidebar/candidatesidebar.component';
import { SidebarComponent as EmpolyerSidebarComponent } from './components/empolyer-sidebar/empolyersidebar.component';
import { LayoutComponent } from './components/empolyer-layout/layout.component';
import { StepIndicatorComponent } from './components/empolyer-step-indicator/step-indicator.component';

/**
 * Convenience barrel for reusable, feature-agnostic UI components.
 * Components are standalone; import them directly or import this module.
 */
@NgModule({
  imports: [
    NavbarComponent,
    PortalSidebarComponent,
    CandidateSidebarComponent,
    EmpolyerSidebarComponent,
    LayoutComponent,
    StepIndicatorComponent,
  ],
  exports: [
    NavbarComponent,
    PortalSidebarComponent,
    CandidateSidebarComponent,
    EmpolyerSidebarComponent,
    LayoutComponent,
    StepIndicatorComponent,
  ],
})
export class SharedModule {}
