import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularFileUploaderModule } from 'angular-file-uploader';
import { ChartsModule } from 'ng2-charts';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BarChartComponent } from './bar-chart/bar-chart.component';
import { PagerComponent } from './components/pager/pager.component';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { TextIputComponent } from './components/text-iput/text-iput.component';
import { LineChartComponent } from './line-chart/line-chart.component';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { NgxPrintModule } from 'ngx-print';
import { NgxBootstrapSwitchModule } from 'ngx-bootstrap-switch';
import { CoreModule } from '../core/core.module';
import { HasClaimDirective } from '../core/security/has-claim.directive';

@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    TextIputComponent,
    BarChartComponent,
    LineChartComponent,
    UploadFileComponent,
    HasClaimDirective,
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    ReactiveFormsModule,
    ChartsModule,
    FormsModule,
    NgSelectModule,
    ModalModule,
    AccordionModule.forRoot(),
    AngularFileUploaderModule,
    NgxPrintModule,
    NgxBootstrapSwitchModule.forRoot(),
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    ReactiveFormsModule,
    BsDropdownModule,
    TextIputComponent,
    BarChartComponent,
    LineChartComponent,
    FormsModule,
    NgSelectModule,
    AccordionModule,
    ModalModule,
    AngularFileUploaderModule,
    UploadFileComponent,
    NgxPrintModule,
    NgxBootstrapSwitchModule,
    HasClaimDirective,
  ],
  providers: [BarChartComponent, LineChartComponent, BsModalService],
})
export class SharedModule {}
