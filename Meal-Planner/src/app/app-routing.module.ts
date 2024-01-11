import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SearchComponent } from './search/search.component';
import { AboutComponent } from './about/about.component';
import { RecipeDetailComponent } from './recipe-detail/recipe-detail.component';
import { CreateRecipeComponent } from './create-recipe/create-recipe.component';
import { UpdateRecipeComponent } from './update-recipe/update-recipe.component';
import { UserLoginComponent } from './user-login/user-login.component';
import { UserRegisterComponent } from './user-register/user-register.component';
import { TestComponent } from './test/test.component';
import { AuthGuard } from './guards/auth.guard';



const routes: Routes = [
  // {path: 'dashboard/:id',  component: DashboardComponent, canActivate: [AuthGuard], data: {animation: 'fader'}},
  {path: 'dashboard',  component: DashboardComponent, canActivate: [AuthGuard], data: {animation: 'fader'}},
  {path: 'search', component: SearchComponent, canActivate: [AuthGuard], data: {animation: 'fader'}},
  {path: 'about', component: AboutComponent, canActivate: [AuthGuard], data: {animation: 'fader'}},
  {path: 'recipe-detail/:id', component: RecipeDetailComponent, canActivate: [AuthGuard], data: {animation: 'fader'} },
  {path: 'create-recipe', component: CreateRecipeComponent, canActivate: [AuthGuard], data: {animation: 'fader'}},
  {path: 'update/:id', component: UpdateRecipeComponent, canActivate: [AuthGuard], data: {animation: 'fader'}},
  {path: 'recipe', redirectTo: '/search', pathMatch: 'full', data: {animation: 'fader'}},
  // more routes goes here
  {path: '', redirectTo: '/login', pathMatch: 'full', data: {animation: 'fader'}},
  {path: 'login', component: UserLoginComponent, data: {animation: 'fader'}},
  {path: 'register', component: UserRegisterComponent, data: {animation: 'fader'}},
  {path: 'test', component: TestComponent, data: {animation: 'fader'}}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
