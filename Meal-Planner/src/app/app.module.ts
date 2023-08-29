import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // Import BrowserAnimationsModule
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { HttpClientModule } from '@angular/common/http'; // Import HttpClientModule
import { ReactiveFormsModule } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SearchComponent } from './search/search.component';
import { AboutComponent } from './about/about.component';
import { WeekScheduleComponent } from './week-schedule/week-schedule.component';
import { BookmarkComponent } from './bookmark/bookmark.component';
import { RecipeCardComponent } from './recipe-card/recipe-card.component';
import { CdkDrag, CdkDropList, CdkDropListGroup, DragDrop, DragDropModule } from '@angular/cdk/drag-drop';
import { ShoppingListComponent } from './shopping-list/shopping-list.component';
import { CustomFilterPipe } from './custom-filter-pipe.pipe';
import { RecipeDetailComponent } from './recipe-detail/recipe-detail.component';
import { CreateRecipeComponent } from './create-recipe/create-recipe.component';
import { FormsTestComponent } from './forms-test/forms-test.component';
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';
import { UpdateRecipeComponent } from './update-recipe/update-recipe.component';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    DashboardComponent,
    SearchComponent,
    AboutComponent,
    WeekScheduleComponent,
    BookmarkComponent,
    RecipeCardComponent,
    ShoppingListComponent,
    CustomFilterPipe,
    RecipeDetailComponent,
    CreateRecipeComponent,
    FormsTestComponent,
    LoadingSpinnerComponent,
    UpdateRecipeComponent,
    
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DragDropModule,
    CdkDropListGroup, CdkDropList, CdkDrag,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
