function filterExercises() {
    const currentMuscleGroupId = muscleGroupSelectList.value;
    const exercises = document.querySelectorAll(".exercise-selection-div");
    const searchQuery = searchInput.value.toLowerCase();

    exercises.forEach(exercise => {
        let para = exercise.getElementsByTagName("p")[0];
        let txtValue = para.textContent.toLowerCase();

        const isSameMuscleGroup = currentMuscleGroupId === "all" || exercise.dataset.mgId === currentMuscleGroupId;
        const matchesQuery = txtValue.includes(searchQuery);

        if (isSameMuscleGroup && matchesQuery) {
            exercise.style.display = "";
        } else {
            exercise.style.display = "none";
        }
    });
}