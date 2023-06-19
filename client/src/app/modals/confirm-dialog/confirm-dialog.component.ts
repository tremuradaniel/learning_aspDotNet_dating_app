import { BsModalRef } from 'ngx-bootstrap/modal';
import { Component } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  result = false;

  constructor(public BsModalRef: BsModalRef) {
    
  }

  ngOnInit(): void {

  }

  confirm() {
    this.result = true;
    this.BsModalRef.hide();
  }

  decline() {
    this.BsModalRef.hide();
  }
}
