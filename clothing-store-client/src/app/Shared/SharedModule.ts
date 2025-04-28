import { CommonModule } from "@angular/common";
import { TruncatePipe } from "../pipe/truncate.pipe";
import { NgModule } from "@angular/core";

@NgModule({
    declarations: [TruncatePipe],
    exports: [TruncatePipe],
    imports: [CommonModule]
  })
  export class SharedModule { }
  