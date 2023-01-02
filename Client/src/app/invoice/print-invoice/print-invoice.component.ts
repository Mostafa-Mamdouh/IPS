import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { invoiceType } from 'src/app/shared/models/Enum';
import { IInvoice } from 'src/app/shared/models/invoice';
import { InvoiceService } from '../invoice.service';

@Component({
  selector: 'app-print-invoice',
  templateUrl: './print-invoice.component.html',
  styleUrls: ['./print-invoice.component.scss'],
})
export class PrintInvoiceComponent implements OnInit {
  footer =
    'info@integrated-ps.com| (+202) 2307880 | ' +
    '١٧١ ش احمد عكاشة - البنفسج ٧ - التجمع الأول - القاهرة الجديدة - القاهرة ';

  @Input() invoice: IInvoice;
  totalInvoiceAmount: number = 0;

  constructor(
    private invoiceService: InvoiceService,
    private router: Router,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {}

  createTotalInvoiceAmount() {
    if (this.invoice) {
      return (this.totalInvoiceAmount = this.invoice.invoiceProducts
        .map((a) => a.price * a.quantity)
        .reduce(function (a, b) {
          return a + b;
        }));
    }
  }
}
