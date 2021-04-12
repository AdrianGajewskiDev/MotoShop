import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrls: ['./image-slider.component.sass']
})
export class ImageSliderComponent implements OnInit {

  constructor() { }

  @Input() Images: string[];
  @Input() ShowNavigationMenu: boolean = true;

  ngOnInit(): void {

    this.addNavigationBtns();
    this.fetchImagesToArray();
    this.setCurrentActivElements();

    for (let i = 0; i < this.navigationButtons.length; i++) {
      this.navigationButtons[i].addEventListener("click", () => {
        let currentIndex = Number.parseInt(this.navigationButtons[i].id.split("-")[1]);
        this.currentImageIndex = currentIndex;
        this.switchImage(currentIndex);
      });
    }

    console.log(this.Images);

  }

  private imagesArray = [];

  images = document.getElementsByClassName("img");
  navigationButtonsContainer = document.querySelector(".nav-list")
  navigationButtons = document.getElementsByClassName("nav-btn");
  leftArrowButton = document.querySelector(".left");
  rightArrowButton = document.querySelector(".right");
  currentImageIndex = 0;

  addNavigationBtns() {
    let count = this.images.length;

    for (let i = 0; i < count; i++) {
      this.navigationButtonsContainer.innerHTML += `<li class='nav-list-item'><button class='nav-btn' id='nav-${i}'></button></li>`;
    }
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
    if (this.currentImageIndex == this.images.length - 1)
      return;

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
