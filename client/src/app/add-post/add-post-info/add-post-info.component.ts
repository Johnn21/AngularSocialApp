import { CdkStepper } from '@angular/cdk/stepper';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-add-post-info',
  templateUrl: './add-post-info.component.html',
  styleUrls: ['./add-post-info.component.css']
})
export class AddPostInfoComponent implements OnInit {
  postForm: FormGroup;

  constructor(private fb: FormBuilder, private postService: PostService, private stepper: CdkStepper) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.postForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
    })
  }

  addPostInfo() {
    this.postService.addPost(this.postForm.value).subscribe((result) => {
      if (result) {
        this.stepper.next();
      }
    })
  }

}
