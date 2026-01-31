"use strict"

window.addEventListener('load', function () {
	document.querySelectorAll('.hidden-on-start').forEach(function (element) {
		element.classList.remove('hidden-on-start');
	});
});

function testWebP(callback) {
	var webP = new Image();
	webP.onload = webP.onerror = function () {
		callback(webP.height == 2);
	};
	webP.src = "data:image/webp;base64,UklGRjoAAABXRUJQVlA4IC4AAACyAgCdASoCAAIALmk0mk0iIiIiIgBoSygABc6WWgAA/veff/0PP8bA//LwYAAA";
}
testWebP(function (support) {
	if (support == true) {
		document.querySelector('body').classList.add('webp');
	} else {
		document.querySelector('body').classList.add('no-webp');
	}
});


function ibg() {

	let ibg = document.querySelectorAll(".ibg");
	for (var i = 0; i < ibg.length; i++) {
		if (ibg[i].querySelector('img')) {
			ibg[i].style.backgroundImage = 'url(' + ibg[i].querySelector('img').getAttribute('src') + ')';
		}
	}
}

ibg();

//=====================================================================

// Бургер

let burgerIcon = document.querySelector(".icon-menu");
let burgerMenu = document.querySelector(".menu__body");
let menuLinks = document.querySelectorAll(".menu__link")

if (burgerIcon) {
	burgerIcon.addEventListener("click", function (e) {
		burgerIcon.classList.toggle('active');
		burgerMenu.classList.toggle('active');
	});

	for (let index = 0; index < menuLinks.length; index++) {
		const menuLink = menuLinks[index];
		menuLink.addEventListener("click", function (e) {
			burgerIcon.classList.remove('active');
			burgerMenu.classList.remove('active');
		});
	}
}

//=====================================================================

// Смена тортиков

var isMobile = {
	Android: function () {
		return navigator.userAgent.match(/Android/i);
	},
	BlackBerry: function () {
		return navigator.userAgent.match(/BlackBerry/i);
	},
	iOS: function () {
		return navigator.userAgent.match(/iPhone|iPad|iPod/i);
	},
	Opera: function () {
		return navigator.userAgent.match(/Opera Mini/i);
	},
	Windows: function () {
		return navigator.userAgent.match(/IEMobile/i) || navigator.userAgent.match(/WPDesktop/i);
	},
	any: function () {
		return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
	}
};

let cakeButton = document.querySelector(".preview__btn");
let cakeFirst = document.querySelector(".preview__cake_first");
let cakeSecond = document.querySelector(".preview__cake_second");
let cakeLeft = document.querySelector(".preview__side-cake-background");


if (!isMobile.any()) {
	if (cakeButton) {
		cakeButton.addEventListener("mouseenter", function (e) {
			cakeSecond.classList.remove('invisible');
			cakeFirst.classList.add('invisible');
			cakeLeft.classList.add('invisible');
		});
		cakeButton.addEventListener("mouseleave", function (e) {
			cakeSecond.classList.add('invisible');
			cakeFirst.classList.remove('invisible');
			cakeLeft.classList.remove('invisible');
		});
	}
}

//=====================================================================

// Слайдер

if (document.querySelector('.slider')) {

	let slider = new Swiper('.slider__body', {
		observer: true,
		observeParents: true,
		slidesPerView: 3,
		//slidesPerView: 'auto',
		slidesPerGroup: 3,
		spaceBetween: 32,
		watchOverflow: true,
		speed: 800,
		loop: true,
		loopAdditionalSlides: 5,
		preloadImages: false,
		parallax: true,

		navigation: {
			nextEl: '.slider .slider__arrow_next',
			prevEl: '.slider .slider__arrow_prev',
		},


		breakpoints: {
			320: {
				slidesPerView: 1,
				spaceBetween: 15,
				slidesPerGroup: 1,
			},
			374.98: {
				slidesPerView: 1,
				spaceBetween: 15,
				slidesPerGroup: 1,
			},
			479.98: {
				slidesPerView: 1,
				spaceBetween: 15,
				slidesPerGroup: 1,
			},
			767.98: {
				slidesPerView: 2,
				spaceBetween: 20,
				slidesPerGroup: 2,
			},
			1110: {
				slidesPerView: 3,
				spaceBetween: 32,
				slidesPerGroup: 3,
			},
			1250: {
				slidesPerView: 3,
				spaceBetween: 32,
				slidesPerGroup: 3,
			},
		}
	});
}

//=====================================================================

// Динамический адаптив

(function () {
	let originalPositions = [];
	let daElements = document.querySelectorAll('[data-da]');
	let daElementsArray = [];
	let daMatchMedia = [];
	//Заполняем массивы
	if (daElements.length > 0) {
		let number = 0;
		for (let index = 0; index < daElements.length; index++) {
			const daElement = daElements[index];
			const daMove = daElement.getAttribute('data-da');
			if (daMove != '') {
				const daArray = daMove.split(',');
				const daPlace = daArray[1] ? daArray[1].trim() : 'last';
				const daBreakpoint = daArray[2] ? daArray[2].trim() : '767';
				const daType = daArray[3] === 'min' ? daArray[3].trim() : 'max';
				const daDestination = document.querySelector('.' + daArray[0].trim())
				if (daArray.length > 0 && daDestination) {
					daElement.setAttribute('data-da-index', number);
					//Заполняем массив первоначальных позиций
					originalPositions[number] = {
						"parent": daElement.parentNode,
						"index": indexInParent(daElement)
					};
					//Заполняем массив элементов
					daElementsArray[number] = {
						"element": daElement,
						"destination": document.querySelector('.' + daArray[0].trim()),
						"place": daPlace,
						"breakpoint": daBreakpoint,
						"type": daType
					}
					number++;
				}
			}
		}
		dynamicAdaptSort(daElementsArray);

		//Создаем события в точке брейкпоинта
		for (let index = 0; index < daElementsArray.length; index++) {
			const el = daElementsArray[index];
			const daBreakpoint = el.breakpoint;
			const daType = el.type;

			daMatchMedia.push(window.matchMedia("(" + daType + "-width: " + daBreakpoint + "px)"));
			daMatchMedia[index].addListener(dynamicAdapt);
		}
	}
	//Основная функция
	function dynamicAdapt(e) {
		for (let index = 0; index < daElementsArray.length; index++) {
			const el = daElementsArray[index];
			const daElement = el.element;
			const daDestination = el.destination;
			const daPlace = el.place;
			const daBreakpoint = el.breakpoint;
			const daClassname = "_dynamic_adapt_" + daBreakpoint;

			if (daMatchMedia[index].matches) {
				//Перебрасываем элементы
				if (!daElement.classList.contains(daClassname)) {
					let actualIndex = indexOfElements(daDestination)[daPlace];
					if (daPlace === 'first') {
						actualIndex = indexOfElements(daDestination)[0];
					} else if (daPlace === 'last') {
						actualIndex = indexOfElements(daDestination)[indexOfElements(daDestination).length];
					}
					daDestination.insertBefore(daElement, daDestination.children[actualIndex]);
					daElement.classList.add(daClassname);
				}
			} else {
				//Возвращаем на место
				if (daElement.classList.contains(daClassname)) {
					dynamicAdaptBack(daElement);
					daElement.classList.remove(daClassname);
				}
			}
		}
		customAdapt();
	}

	//Вызов основной функции
	dynamicAdapt();

	//Функция возврата на место
	function dynamicAdaptBack(el) {
		const daIndex = el.getAttribute('data-da-index');
		const originalPlace = originalPositions[daIndex];
		const parentPlace = originalPlace['parent'];
		const indexPlace = originalPlace['index'];
		const actualIndex = indexOfElements(parentPlace, true)[indexPlace];
		parentPlace.insertBefore(el, parentPlace.children[actualIndex]);
	}
	//Функция получения индекса внутри родителя
	function indexInParent(el) {
		var children = Array.prototype.slice.call(el.parentNode.children);
		return children.indexOf(el);
	}
	//Функция получения массива индексов элементов внутри родителя
	function indexOfElements(parent, back) {
		const children = parent.children;
		const childrenArray = [];
		for (let i = 0; i < children.length; i++) {
			const childrenElement = children[i];
			if (back) {
				childrenArray.push(i);
			} else {
				//Исключая перенесенный элемент
				if (childrenElement.getAttribute('data-da') == null) {
					childrenArray.push(i);
				}
			}
		}
		return childrenArray;
	}
	//Сортировка объекта
	function dynamicAdaptSort(arr) {
		arr.sort(function (a, b) {
			if (a.breakpoint > b.breakpoint) { return -1 } else { return 1 }
		});
		arr.sort(function (a, b) {
			if (a.place > b.place) { return 1 } else { return -1 }
		});
	}

	function customAdapt() {
		//const viewport_width = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
	}
}());

//=====================================================================

// Попап

const popupLinks = document.querySelectorAll('.popup-link');
const body = document.querySelector('body');
const lockPadding = document.querySelectorAll(".lock-padding");

let unlock = true;

// 800 милисекунд - должна совпадать со значением в св-ве transition
const timeout = 800;

if (popupLinks.length > 0) {
	for (let index = 0; index < popupLinks.length; index++) {
		const popupLink = popupLinks[index];
		popupLink.addEventListener("click", function (e) {
			const popupName = popupLink.getAttribute('href').replace('#', '');
			const curentPopup = document.getElementById(popupName);
			popupOpen(curentPopup);
			e.preventDefault();
		});
	}
}

const popupCloseIcon = document.querySelectorAll('.close-popup');
if (popupCloseIcon.length > 0) {
	for (let index = 0; index < popupCloseIcon.length; index++) {
		const el = popupCloseIcon[index];
		el.addEventListener('click', function (e) {
			popupClose(el.closest('.popup'));
			e.preventDefault();
		});
	}
}

function popupOpen(curentPopup) {
	if (curentPopup && unlock) {
		const popupActive = document.querySelector('.popup.open');
		if (popupActive) {
			popupClose(popupActive, false);
		} else {
			bodyLock();
		}
		curentPopup.classList.add('open');
		/*
				curentPopup.addEventListener("click", function (e) {
					if (!e.target.closest('.popup__content')) {
						popupClose(e.target.closest('.popup'));
					}
				});
		*/
	}
}

function popupClose(popupActive, doUnlock = true) {
	if (unlock) {
		popupActive.classList.remove('open');
		if (doUnlock) {
			bodyUnLock();
		}
	}
}

function сloseAllPopups() {
	document.querySelectorAll('.popup').forEach(popupClose);
}

function bodyLock() {
	const lockPaddingValue = window.innerWidth - document.querySelector('.wrapper').offsetWidth + 'px';

	if (lockPadding.length > 0) {
		for (let index = 0; index < lockPadding.length; index++) {
			const el = lockPadding[index];
			el.style.paddingRight = lockPaddingValue;
		}
		body.style.paddingRight = lockPaddingValue;
		body.classList.add('lock');

		unlock = false;
		setTimeout(function () {
			unlock = true;
		}, timeout);
	}
}

function bodyUnLock() {
	setTimeout(function () {
		if (lockPadding.length > 0) {
			for (let index = 0; index < lockPadding.length; index++) {
				const el = lockPadding[index];
				el.style.paddingRight = '0px';
			}
		}
		body.style.paddingRight = '0px';
		body.classList.remove('lock');
	}, timeout);

	unlock = false;
	setTimeout(function () {
		unlock = true;
	}, timeout);
}

document.addEventListener('keydown', function (e) {
	if (e.which === 27) {
		const popupActive = document.querySelector('.popup.open');
		popupClose(popupActive);
	}
});

(function () {
	if (!Element.prototype.closest) {
		Element.prototype.closest = function (css) {
			var node = this;
			while (node) {
				if (node.matches(css)) return node;
				else node = node.parentElement
			}
			return null;
		};
	}
})();
(function () {
	if (!Element.prototype.matches) {
		Element.prototype.matches = Element.prototype.matchesSelector ||
			Element.prototype.webkitMatchesSelector ||
			Element.prototype.mozMatchesSelector ||
			Element.prototype.msMatchesSelector;
	}
})();

//=====================================================================

// Выбор формы повар/клиент

let respondentClient = document.querySelector(".popup__respondent_client");
let respondentCook = document.querySelector(".popup__respondent_cook");
let formClient = document.querySelector(".form_client");
let formCook = document.querySelector(".form_cook");

if (document.querySelector(".popup__respondents")) {

	respondentClient.addEventListener("click", function (e) {
		respondentClient.classList.add('active');
		respondentCook.classList.remove('active');
		formClient.classList.remove('invisible-item');
		formCook.classList.add('invisible-item');
	});

	respondentCook.addEventListener("click", function (e) {
		respondentClient.classList.remove('active');
		respondentCook.classList.add('active');
		formClient.classList.add('invisible-item');
		formCook.classList.remove('invisible-item');
	});

}

//=====================================================================

// Отправка на почту (клиент)

let clientSendBtn = document.querySelector(".client-send-btn");

if (document.querySelector('.form')) {

	document.addEventListener('DOMContentLoaded', function () {
		const form = document.getElementById('client-form');
		form.addEventListener('submit', formSend);

		async function formSend(e) {
			e.preventDefault();

			let errorEmail = formValidateEmail(form);
			let error = formValidate(form);

			let formData = new FormData(form);
			//formData.append('image', formImage.files[0]);
            console.log(form);
			console.log(formData);
			if (errorEmail === 0) {
				if (error === 0) {
					form.classList.add('_sending');
					let response = await fetch('https://dev.dishnfork.com/api/v1/messaging/send-from-lending', {
						headers: {
							'Accept': 'application/json',
							'Content-Type': 'application/json'
						  },
						method: 'POST',
						body: JSON.stringify({ type:'Client', name: formData.get('client-name'), email:formData.get('client-email'), message:formData.get('client-message')})
					});
					if (response.ok) {
						form.reset();
						form.classList.remove('_sending');
						сloseAllPopups();
					} else {
						alert("Ошибка");
						form.classList.remove('_sending');
					}
				} else {
					//alert('Заполните обязательные поля')
				}
			} else {
				//alert('Электронный адрес заполнен некорректно / незаполнены обязательные поля');
			}
		}


		function formValidateEmail(form) {
			let errorEmail = 0;
			let formReq = document.querySelectorAll('.req')

			for (let index = 0; index < formReq.length; index++) {
				const input = formReq[index];
				formRemoveError(input);

				if (input.classList.contains('email')) {
					if (emailTest(input)) {
						formAddError(input);
						errorEmail++;
					}
				}
			}
			return errorEmail;
		}

		function formValidate(form) {
			let error = 0;
			let formReq = document.querySelectorAll('.req')

			for (let index = 0; index < formReq.length; index++) {
				const input = formReq[index];
				formRemoveError(input);

				if (input.classList.contains('email')) {
					if (emailTest(input)) {
						formAddError(input);
						error++;
					}
				} else if (input.getAttribute("type") === "checkbox" && input.checked === false) {
					formAddError(input);
					error++;
				} else {
					if (input.value === '') {
						formAddError(input);
						error++;
					}
				}
			}
			return error;
		}
		function formAddError(input) {
			input.parentElement.classList.add('error');
			input.classList.add('error');
			clientSendBtn.classList.add('btn_disabled');
		}
		function formRemoveError(input) {
			input.parentElement.classList.remove('error');
			input.classList.remove('error');
			clientSendBtn.classList.remove('btn_disabled');
		}
		function emailTest(input) {
			return !/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,8})+$/.test(input.value);
		}

		const formImage = document.getElementById('formImage');
		const formPreview = document.getElementById('formPreview');
		//formImage.addEventListener('change', () => {
		//	uploadFile(formImage.files[0]);
		//});

		function uploadFile(file) {
			if (!['image/jpeg', 'image/png', 'image/gif'].includes(file.type)) {
				alert('Разрешены только изображения.');
				formImage.value = '';
				return;
			}
			if (file.size > 2 * 1024 * 1024) {
				alert('Файл должен быть не более 2 МБ.');
				return;
			}

			var reader = new FileReader();
			reader.onload = function (e) {
				formPreview.innerHTML = `<img src="${e.target.result}" alt="Фото">`;
			};
			reader.onerror = function (e) {
				alert('Ошибка');
			};
			reader.readAsDataURL(file);
		}
	});

}

//=====================================================================


// Отправка на почту (повар)

let cookSendBtn = document.querySelector(".cook-send-btn");

if (document.querySelector('.form')) {

	document.addEventListener('DOMContentLoaded', function () {
		const form = document.getElementById('cook-form');
		form.addEventListener('submit', formSend);

		async function formSend(e) {
			e.preventDefault();

			let errorEmail = formValidateEmail(form);
			let error = formValidate(form);

			let formData = new FormData(form);
			//formData.append('image', formImage.files[0]);

			if (errorEmail === 0) {
				if (error === 0) {
					form.classList.add('_sending');
					let response = await fetch('https://dev.dishnfork.com/api/v1/messaging/send-from-lending', {
						headers: {
							'Accept': 'application/json',
							'Content-Type': 'application/json'
						  },
						method: 'POST',
						body: JSON.stringify({ type:'Cook', name: formData.get('cook-name'), email:formData.get('cook-email'), message:formData.get('cook-message')})
					});
					if (response.ok) {
						form.reset();
						form.classList.remove('_sending');
						сloseAllPopups();
					} else {
						alert("Ошибка");
						form.classList.remove('_sending');
					}
				} else {
					//alert('Заполните обязательные поля')
				}
			} else {
				//alert('Электронный адрес заполнен некорректно / незаполнены обязательные поля');
			}
		}


		function formValidateEmail(form) {
			let errorEmail = 0;
			let formReq = document.querySelectorAll('.req-1')

			for (let index = 0; index < formReq.length; index++) {
				const input = formReq[index];
				formRemoveError(input);

				if (input.classList.contains('email-1')) {
					if (emailTest(input)) {
						formAddError(input);
						errorEmail++;
					}
				}
			}
			return errorEmail;
		}

		function formValidate(form) {
			let error = 0;
			let formReq = document.querySelectorAll('.req-1')

			for (let index = 0; index < formReq.length; index++) {
				const input = formReq[index];
				formRemoveError(input);

				if (input.classList.contains('email-1')) {
					if (emailTest(input)) {
						formAddError(input);
						error++;
					}
				} else if (input.getAttribute("type") === "checkbox" && input.checked === false) {
					formAddError(input);
					error++;
				} else {
					if (input.value === '') {
						formAddError(input);
						error++;
					}
				}
			}
			return error;
		}
		function formAddError(input) {
			input.parentElement.classList.add('error');
			input.classList.add('error');
			cookSendBtn.classList.add('btn_disabled');
		}
		function formRemoveError(input) {
			input.parentElement.classList.remove('error');
			input.classList.remove('error');
			cookSendBtn.classList.remove('btn_disabled');
		}
		function emailTest(input) {
			return !/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,8})+$/.test(input.value);
		}

		const formImage = document.getElementById('formImage');
		const formPreview = document.getElementById('formPreview');
		//formImage.addEventListener('change', () => {
		//	uploadFile(formImage.files[0]);
		//});

		function uploadFile(file) {
			if (!['image/jpeg', 'image/png', 'image/gif'].includes(file.type)) {
				alert('Разрешены только изображения.');
				formImage.value = '';
				return;
			}
			if (file.size > 2 * 1024 * 1024) {
				alert('Файл должен быть не более 2 МБ.');
				return;
			}

			var reader = new FileReader();
			reader.onload = function (e) {
				formPreview.innerHTML = `<img src="${e.target.result}" alt="Фото">`;
			};
			reader.onerror = function (e) {
				alert('Ошибка');
			};
			reader.readAsDataURL(file);
		}
	});

}

//=====================================================================
