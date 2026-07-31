function createSetDiv(container, exIndex) {
    let setIndex = container.querySelectorAll(".setRow").length;
    let setNumber = setIndex + 1;
    let setContainer = container.querySelector(".setDiv");

    let newSetDiv = document.createElement("div");
    newSetDiv.classList.add("setRow", "mb-2", "align-items-center");
    newSetDiv.innerHTML = `
                <div class="row justify-content-center align-items-center">
                    <div class="col-2">
                        <input class="form-control form-control-sm text-center fw-bold"
                            readonly 
                            type="number"
                            name="Exercises[${exIndex}].Sets[${setIndex}].SetNumber"
                            value="${setNumber}" />
                    </div>
                    <div class="col-4">
                        <input class="form-control form-control-sm text-center"
                            type="number"
                            required
                            name="Exercises[${exIndex}].Sets[${setIndex}].Weight"
                            min="0" />
                    </div>
                    <div class="col-4">
                        <input class="form-control form-control-sm text-center"
                            type="number"
                            required
                            name="Exercises[${exIndex}].Sets[${setIndex}].Repetitions"
                            min="1"/>
                    </div>
                    <div class="col-2">
                        <button type="button" class="btn-close removeSetBtn"></button>
                    </div>
                </div>
            `;
    setContainer.appendChild(newSetDiv);
}

//renumber exercise div ids and input names for model binding after add/remove
function renumberExerciseIndex() {
    let exerciseDivs = exerciseContainer.querySelectorAll(".exerciseDiv");
    exerciseDivs.forEach((exerciseDiv, index) => {
        exerciseDiv.id = `exercise.${index}`; 
        if (exerciseDiv.querySelector("input[type='hidden']") !== null) {
            let hiddenIdInput = exerciseDiv.querySelector("input[type='hidden']");
            hiddenIdInput.name = `Exercises[${index}].Id`;
        }
        let selectInput = exerciseDiv.querySelector("select");
        selectInput.name = `Exercises[${index}].ExerciseId`;
        //renumber set inputs to ensure hidden values are still correct
        let setRows = exerciseDiv.querySelectorAll(".setRow");
        setRows.forEach((setRow, setIndex) => {
            if (setRow.querySelector("input[type='hidden']") !== null) {
                let hiddenSetIdInput = setRow.querySelector("input[type='hidden']");
                hiddenSetIdInput.name = `Exercises[${index}].Sets[${setIndex}].Id`;
            }
            let setNumberInput = setRow.querySelector("input[name$='.SetNumber']");
            setNumberInput.name = `Exercises[${index}].Sets[${setIndex}].SetNumber`;
            let weightInput = setRow.querySelector("input[name$='.Weight']");
            weightInput.name = `Exercises[${index}].Sets[${setIndex}].Weight`;
            let repsInput = setRow.querySelector("input[name$='.Repetitions']");
            repsInput.name = `Exercises[${index}].Sets[${setIndex}].Repetitions`;
        });
    });
}

function renumberSets() {
    let exerciseDivs = document.querySelectorAll(".exerciseDiv");
    exerciseDivs.forEach((exerciseDiv) => {
        let setRows = exerciseDiv.querySelectorAll(".setRow");
        setRows.forEach((setRow, index) => {
            let setNumberInput = setRow.querySelector("input[name$='.SetNumber']");
            setNumberInput.value = index + 1;
            let exerciseDiv = setRow.closest(".exerciseDiv");
            let exIndex = parseInt(exerciseDiv.id.split(".")[1]);
            if (setRow.querySelector("input[type='hidden']") !== null) {
                let hiddenSetIdInput = setRow.querySelector("input[type='hidden']");
                hiddenSetIdInput.name = `Exercises[${exIndex}].Sets[${index}].Id`;
            }
            setNumberInput.name = `Exercises[${exIndex}].Sets[${index}].SetNumber`;
            let weightInput = setRow.querySelector("input[name$='.Weight']");
            weightInput.name = `Exercises[${exIndex}].Sets[${index}].Weight`;
            let repsInput = setRow.querySelector("input[name$='.Repetitions']");
            repsInput.name = `Exercises[${exIndex}].Sets[${index}].Repetitions`;
        });
    });
}