import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home {
  title = "Welcome to FinanceTracker";
  description = "Manage your finances in one place";
  balance = 1500;
  amount = 0;

  addMoney() {
    this.balance += this.amount;
  }
}
