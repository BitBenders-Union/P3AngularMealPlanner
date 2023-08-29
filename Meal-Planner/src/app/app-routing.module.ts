import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SearchComponent } from './search/search.component';
import { AboutComponent } from './about/about.component';
import { RecipeDetailComponent } from './recipe-detail/recipe-detail.component';
import { CreateRecipeComponent } from './create-recipe/create-recipe.component';
import { FormsTestComponent } from './forms-test/forms-test.component';
import { UpdateRecipeComponent } from './update-recipe/update-recipe.component';



const routes: Routes = [
  {path: 'dashboard', component: DashboardComponent, data: {animation: 'fader'}},
  {path: 'search', component: SearchComponent, data: {animation: 'fader'}},
  {path: 'about', component: AboutComponent, data: {animation: 'fader'}},
  {path: 'recipe-detail/:id', component: RecipeDetailComponent, data: {animation: 'fader'} }, // New route
  {path: 'create-recipe', component: CreateRecipeComponent, data: {animation: 'fader'}},
  {path: 'forms-test', component: FormsTestComponent, data: {animation: 'fader'}},
  {path: 'update/:id', component: UpdateRecipeComponent, data: {animation: 'fader'}},
  {path: 'recipe', redirectTo: '/search', pathMatch: 'full', data: {animation: 'fader'}},
  // more routes goes here
  {path: '', redirectTo: '/dashboard', pathMatch: 'full', data: {animation: 'fader'}}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
