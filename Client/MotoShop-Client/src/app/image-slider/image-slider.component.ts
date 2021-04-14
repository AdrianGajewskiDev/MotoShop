import { AfterViewInit, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrls: ['./image-slider.component.sass']
})
export class ImageSliderComponent implements OnInit, AfterViewInit {
  @Input() ImagesUrls: string[] = [];
  @Input() ShowNavigationMenu: boolean = true;
  images;
  navigationButtonsContainer;
  navigationButtons;
  leftArrowButton;
  rightArrowButton;
  currentImageIndex = 0;
  imagesArray = [];

  constructor() { }

  ngOnInit() {
    this.images = document.getElementsByClassName("img");

  }

  ngAfterViewInit(): void {

    this.navigationButtonsContainer = document.querySelector(".nav-list")
    this.navigationButtons = document.getElementsByClassName("nav-btn");
    this.leftArrowButton = document.querySelector(".left");
    this.rightArrowButton = document.querySelector(".right");

    this.fetchImagesToArray();
    this.setCurrentActivElements();

    for (let i = 0; i < this.navigationButtons.length; i++) {
      this.navigationButtons[i].addEventListener("click", () => {
        let currentIndex = Number.parseInt(this.navigationButtons[i].id.split("-")[1]);
        this.currentImageIndex = currentIndex;
        this.switchImage(currentIndex);
      });
    }

    this.leftArrowButton.addEventListener("click", () => {
      this.previousImage();
    });

    this.rightArrowButton.addEventListener("click", () => {
      this.nextImage();
    });
  }


  setCurrentActivElements() {
    this.imagesArray[0].element.classList.add("img-active")
    this.navigationButtons[0].classList.add("active");
  }

  fetchImagesToArray() {
    for (let i = 0; i < this.images.length; i++) {
      this.imagesArray.push({ index: i, element: this.images[i], navBtn: document.getElementById(`nav-${i}`) });
    }
  }

  switchImage(index) {
    for (let i = 0; i < this.images.length; i++) {
      if (this.imagesArray[i].index == index) {
        this.imagesArray[i].element.classList.add("img-active");
        this.imagesArray[i].navBtn.classList.add("active");
      }
      else {
        this.imagesArray[i].element.classList.remove("img-active");
        this.imagesArray[i].navBtn.classList.remove("active");
      }
    }
  }

  nextImage() {
    console.log("jere");

    if (this.currentImageIndex == this.images.length - 1)
      return;
    console.log(this.imagesArray);

    this.currentImageIndex += 1;
    this.switchImage(this.currentImageIndex);
  }

  previousImage() {
    if (this.currentImageIndex == 0)
      return;

    this.currentImageIndex -= 1;
    this.switchImage(this.currentImageIndex);
  }
}
