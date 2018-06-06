using MiEscuela.BIZ;
using MiEscuela.COMMON.Entidades;
using MiEscuela.COMMON.Interfaces;
using MiEscuela.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiEscuela.Vistas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewMateria : ContentPage
	{
        Materia materia;
        IMateriaManager materiaManager;
        bool nuevo;
		public ViewMateria (Materia m, bool n)
		{
            materia = m;
            nuevo = n;
			InitializeComponent ();

            materiaManager = new MateriaManager(new GenericRepository<Materia>());
            contenedor.BindingContext = materia;
            Appearing += ViewMateria_Appering;

            btnAgregarTarea.Clicked += (sender, e) =>
            {
                Navigation.PushAsync(new ViewTarea(new Tarea
                {
                    IdTarea = materia.Id
                },true));
            };

            btnGuardar.Clicked += (sender, e) =>
            {
                if (nuevo)
                {
                    materia = materiaManager.Agregar(contenedor.BindingContext as Materia);
                }
                else
                {
                    materia = materiaManager.Modificar(contenedor.BindingContext as Materia);
                }
                if (materia != null)
                {
                    DisplayAlert("Mi Escuela", "Materia Guardada", "OK");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Mi Escuela", "Error al Guardar", "OK");
                }

            };

            btnEliminar.Clicked += (sender, e) =>
            {
                if (materiaManager.Eliminar(materia.Id))
                {
                    DisplayAlert("Mi Escuela", "Materia Eliminada Correctamente", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Mi Escuela", "No se pudo eliminar la materia", "Ok");
                }
            };
        }

        private void ViewMateria_Appering(object sender, EventArgs e)
        {
            ITareaManager tareaManager = new TareaManager(new GenericRepository<Tarea>());
            lstTareas.ItemsSource = null;
            lstTareas.ItemsSource = tareaManager.BuscarTareasDeMateria(materia.Id);

        }
    }
}