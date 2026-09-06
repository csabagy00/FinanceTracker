import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Transaction } from '../../models/transaction';
import { TransactionService } from '../../services/transactionService';

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
  transactions: Transaction[] = [];
  
  constructor(private transactionService: TransactionService) {
    this.transactions = transactionService.getTransactions();
  }

  addMoney() {
    this.balance += this.amount;
  }
}
