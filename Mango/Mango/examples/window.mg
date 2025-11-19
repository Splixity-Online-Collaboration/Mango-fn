window "Mango UI Window" 400 300 "icon.png" {
  //let counter = 0

  state {
    count = 0
    isStillWip = true // yes it is.
    fancyRedText = "- R E D -"
    fancyBlueText = "- B L U E -"
  }

  row {
    height: 200
    bgcolor: red
    id: container

  }
  
  // @concept  
  //button "+" {
  //  onclick: countUp
  //}

  button "red" {
    id: redButton
    onclick: setRed 
  }
  button "blue" {
    id: blueButton
    onclick: setBlue      
  }

  button "set fancy text" {
    onclick: setFancyText
  }

  // @concept
  //function countUp(){
  //  counter += 1
  //}

  function setRed(){
    update(container, {
      bgcolor: red
    })
  }

  // var expr testing
  function setFancyText(){
    set(label, blueButton, fancyBlueText)
    set(label, redButton, fancyRedText)
  }
  
  function setBlue(){
    update(container, {
      bgcolor: blue
    })
  }
}
