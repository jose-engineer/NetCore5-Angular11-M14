import { RouterModule } from "@angular/router";
import { CheckoutPage } from "../pages/checkout.component";
import { LoginPage } from "../pages/loginPage.component";
import { ShopPage } from "../pages/shopPage.component";
import { AuthActivator } from "../services/authActivator.service";

const routes = [
  { path: "", component: ShopPage },
  { path: "checkout", component: CheckoutPage, canActivate: [AuthActivator] },
  { path: "login", component: LoginPage },
  { path: "**", redirectTo: "/" } //fallback route (catch all): if none of the other routes work, redirect to... ShopPage
];

const router = RouterModule.forRoot(routes, {
  useHash: false //if true, use a # in the url for linking pages
});

export default router;