import { Injectable } from '@angular/core';
import { Transaction } from '../models/transaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private transactions: Transaction[] = [
    { description: 'Salary', amount: 2500},
    { description: 'Groceries', amount: -150},
    { description: 'Netflix', amount: -15},
    { description: 'Coffee', amount: -4.50}
  ];

  getTransactions(): Transaction[]{
    return this.transactions;
  }
}
